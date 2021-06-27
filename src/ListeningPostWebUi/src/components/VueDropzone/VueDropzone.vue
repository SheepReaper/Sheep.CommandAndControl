<template lang="pug">
//- div(ref='dropzonePlaceholder')
div(
  ref='dropzoneElement',
  :class='{ "vue-dropzone dropzone": includeStyling }'
)
  .dz-message(v-if='useCustomSlot')
    slot Drop files here to upload
</template>

<script>
import { computed, defineComponent, onBeforeUnmount, onMounted, ref } from 'vue'
import { generateSignedUrl } from './aws'

import Dropzone from 'dropzone'

const eventMap = {
  addedfiles: 'vdropzone-files-added',
  canceled: 'vdropzone-canceled',
  canceledmultiple: 'vdropzone-canceled-multiple',
  complete: 'vdropzone-complete',
  completemultiple: 'vdropzone-complete-multiple',
  dragend: 'vdropzone-drag-end',
  dragenter: 'vdropzone-drag-enter',
  dragleave: 'vdropzone-drag-leave',
  dragover: 'vdropzone-drag-over',
  dragstart: 'vdropzone-drag-start',
  drop: 'vdropzone-drop',
  error: 'vdropzone-error',
  errormultiple: 'vdropzone-error-multiple',
  maxfilesexceeded: 'vdropzone-max-files-exceeded',
  maxfilesreached: 'vdropzone-max-files-reached',
  processing: 'vdropzone-processing',
  processingmultiple: 'vdropzone-processing-multiple',
  queuecomplete: 'vdropzone-queue-complete',
  reset: 'vdropzone-reset',
  sending: 'vdropzone-sending',
  sendingmultiple: 'vdropzone-sending-multiple',
  success: 'vdropzone-success',
  successmultiple: 'vdropzone-success-multiple',
  thumbnail: 'vdropzone-thumbnail',
  totaluploadprogress: 'vdropzone-total-upload-progress',
  uploadprogress: 'vdropzone-upload-progress'
}

export const VueDropzone = defineComponent({
  props: {
    options: {
      type: Object,
      required: true
    },
    includeStyling: {
      type: Boolean,
      default: true,
      required: false
    },
    awss3: {
      type: Object,
      required: false,
      default: null
    },
    destroyDropzone: {
      type: Boolean,
      default: true,
      required: false
    },
    duplicateCheck: {
      type: Boolean,
      default: false,
      required: false
    },
    useCustomSlot: {
      type: Boolean,
      default: true,
      required: false
    },
    confirm: {
      type: Function,
      default: null,
      required: false
    }
  },
  emits: [
    ...Object.values(eventMap),
    'vdropzone-file-added-manually',
    'vdropzone-file-added',
    'vdropzone-removed-file',
    'vdropzone-mounted'
  ],
  setup: (props, { emit }) => {
    const proxyMethods = [
      'accept',
      'addFile',
      'destroy',
      'disable',
      'enable',
      'filesize',
      'getAcceptedFiles',
      'getActiveFiles',
      'getAddedFiles',
      'getExistingFallback',
      'getFallbackForm',
      'getFilesWithStatus',
      'getQueuedFiles',
      'getRejectedFiles',
      'getUploadingFiles',
      'init',
      'removeEventListeners',
      'removeAllFiles',
      'removeFile',
      'setupEventListeners',
      'updateTotalUploadProgress'
    ]

    Dropzone.autoDiscover = false

    const isS3 = computed(() => props.awss3 !== null)

    let hasBeenMounted = false

    const dropzoneSettings = computed(() => ({
      thumbnailWidth: 200,
      thumbnailHeight: 200,
      ...props.options
    }))

    const s3DropZoneSettings = computed(() => ({
      ...dropzoneSettings.value,
      method: 'PUT',
      parallelUploads: 1,
      uploadMultiple: false,
      paramName: 'files',
      autoProcessQueue: false,
      sending: (file, xhr) => xhr.send(file),
      accept: async (
        /** @type {import('dropzone').DropzoneFile} */ file,
        done
      ) => {
        if (isS3.value) {
          generateSignedUrl(
            props.awss3.signingURL,
            file,
            props.awss3.includeFile
          ).then((signed) => {
            setOption('headers', {
              'Content-Type': file.type,
              'x-amz-acl': 'public-read'
            })
            setOption('url', signed.signature)
            done()
            setTimeout(() => dropzoneElement.value.dropzone.processFile(file))
          })
        }
      }
    }))

    if (props.confirm) Dropzone.confirm = () => props.confirm

    const dropzoneElement = ref(null)

    const setOption = (option, value) =>
      (dropzoneElement.value.dropzone.options[option] = value)

    const proxiedMethods = proxyMethods.reduce(
      (o, name) => (o[name] = () => dropzoneElement.value.dropzone[name]),
      {}
    )

    const manuallyAddFile = (file, fileUrl) => {
      file.manuallyAdded = true
      dropzoneElement.value.dropzone.emit('addedfile', file)
      let containsImageFileType = false
      const supportedThumbnailTypes = [
        '.svg',
        '.png',
        '.jpg',
        'jpeg',
        '.gif',
        '.webp',
        'image/'
      ]
      if (
        supportedThumbnailTypes.filter(
          (s) => fileUrl.toLowerCase().indexOf(s) > -1
        ).length > 0
      ) {
        containsImageFileType = true
      }

      if (
        dropzoneElement.value.dropzone.options.createImageThumbnails &&
        containsImageFileType &&
        file.size <=
          dropzoneElement.value.dropzone.options.maxThumbnailFilesize * 1024 * 1024
      ) {
        fileUrl && dropzoneElement.value.dropzone.emit('thumbnail', file, fileUrl)
        var thumbnails = file.previewElement.querySelectorAll(
          '[data-dz-thumbnail]'
        )
        thumbnails.map((tn) => {
          tn.style.width = dropzoneSettings.value.thumbnailWidth + 'px'
          tn.style.height = dropzoneSettings.value.thumbnailHeight + 'px'
          tn.classList.add('vdManualThumbnail')
        })
      }

      dropzoneElement.value.dropzone.emit('complete', file)
      if (dropzoneElement.value.dropzone.options.maxFiles)
        dropzoneElement.value.dropzone.options.maxFiles--
      file.accepted = true
      dropzoneElement.value.dropzone.files.push(file)
      emit('vdropzone-file-added-manually', file)
    }

    const processQueue = () => {
      dropzoneElement.value.dropzone.processQueue()
      dropzoneElement.value.dropzone.on('success', () => {
        dropzoneElement.value.dropzone.options.autoProcessQueue = true
      })
      dropzoneElement.value.dropzone.on('queuecomplete', () => {
        dropzoneElement.value.dropzone.options.autoProcessQueue = false
      })
    }

    onMounted(() => {
      if (hasBeenMounted) return

      hasBeenMounted = true

      const dropzone = new Dropzone(
        dropzoneElement.value,
        isS3.value ? s3DropZoneSettings.value : dropzoneSettings.value
      )

      for (const event in eventMap) {
        dropzone.on(event, (...args) => emit(eventMap[event], ...args))
      }

      dropzone.on('addedfile', async (file) => {
        // let isDuplicate = false
        if (props.duplicateCheck) {
          if (dropzone.files.length) {
            if (
              dropzone.files.some(
                (existing) =>
                  existing.name === file.name &&
                  existing.size === file.size &&
                  existing.lastModified === file.lastModified
              )
            ) {
              dropzone.removeFile(file)
              // isDuplicate = true
              emit('vdropzone-duplicate-file', file)
            }
          }
        }

        emit('vdropzone-file-added', file)
      })

      dropzone.on('removedfile', (file) => {
        emit('vdropzone-removed-file', file)
        if (
          'manuallyAdded' in file &&
          file['manuallyAdded'] &&
          dropzone.options.maxFiles !== null
        ) {
          dropzone.options.maxFiles++
        }
      })

      emit('vdropzone-mounted')
    })

    onBeforeUnmount(() => {
      if (props.destroyDropzone) dropzoneElement.value.dropzone.destroy()
    })

    return {
      dropzoneElement,
      isS3,
      s3DropZoneSettings,
      dropzoneSettings,
      manuallyAddFile,
      setOption,
      processQueue,
      ...proxiedMethods
    }
  }
})

export { VueDropzone as default }
</script>

<style>
.vue-dropzone {
  border: 2px solid #e5e5e5;
  font-family: "Arial", sans-serif;
  letter-spacing: 0.2px;
  color: #777;
  transition: 0.2s linear;
}
.vue-dropzone:hover {
  background-color: #f6f6f6;
}
.vue-dropzone > i {
  color: #ccc;
}
.vue-dropzone > .dz-preview .dz-image {
  border-radius: 0;
  width: 100%;
  height: 100%;
}
.vue-dropzone > .dz-preview .dz-image img:not([src]) {
  width: 200px;
  height: 200px;
}
.vue-dropzone > .dz-preview .dz-image:hover img {
  transform: none;
  -webkit-filter: none;
}
.vue-dropzone > .dz-preview .dz-details {
  bottom: 0;
  top: 0;
  color: white;
  background-color: rgba(33, 150, 243, 0.8);
  transition: opacity 0.2s linear;
  text-align: left;
}
.vue-dropzone > .dz-preview .dz-details .dz-filename {
  overflow: hidden;
}
.vue-dropzone > .dz-preview .dz-details .dz-filename span,
.vue-dropzone > .dz-preview .dz-details .dz-size span {
  background-color: transparent;
}
.vue-dropzone > .dz-preview .dz-details .dz-filename:not(:hover) span {
  border: none;
}
.vue-dropzone > .dz-preview .dz-details .dz-filename:hover span {
  background-color: transparent;
  border: none;
}
.vue-dropzone > .dz-preview .dz-progress .dz-upload {
  background: #cccccc;
}
.vue-dropzone > .dz-preview .dz-remove {
  position: absolute;
  z-index: 30;
  color: white;
  margin-left: 15px;
  padding: 10px;
  top: inherit;
  bottom: 15px;
  border: 2px white solid;
  text-decoration: none;
  text-transform: uppercase;
  font-size: 0.8rem;
  font-weight: 800;
  letter-spacing: 1.1px;
  opacity: 0;
}
.vue-dropzone > .dz-preview:hover .dz-remove {
  opacity: 1;
}
.vue-dropzone > .dz-preview .dz-success-mark,
.vue-dropzone > .dz-preview .dz-error-mark {
  margin-left: auto;
  margin-top: auto;
  width: 100%;
  top: 35%;
  left: 0;
}
.vue-dropzone > .dz-preview .dz-success-mark svg,
.vue-dropzone > .dz-preview .dz-error-mark svg {
  margin-left: auto;
  margin-right: auto;
}
.vue-dropzone > .dz-preview .dz-error-message {
  margin-left: auto;
  margin-right: auto;
  left: 0;
  width: 100%;
  text-align: center;
}
.vue-dropzone > .dz-preview .dz-error-message:after {
  display: none;
}
.vue-dropzone > .dz-preview .vdManualThumbnail {
  object-fit: cover;
}
</style>
