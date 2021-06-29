<template lang="pug">
div(ref='dropzoneElement', :class='{ "vue-dropzone dropzone": includeStyling }'): slot: .dz-message Drop files here to upload
</template>

<script lang="ts">
import { computed, defineComponent, onBeforeUnmount, onMounted, ref } from 'vue'
import { generateSignedUrl } from './aws'

import Dropzone, { DropzoneFile } from 'dropzone'

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
    confirm: {
      type: Function,
      default: null,
      required: false
    }
  },
  emits: [
    ...Object.values(eventMap),
    'vdropzone-duplicate-file',
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
      accept: async (file: DropzoneFile, done: () => void) => {
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

    const manuallyAddFile = (file: DropzoneFile, fileUrl) => {
      file['manuallyAdded'] = true
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
          dropzoneElement.value.dropzone.options.maxThumbnailFilesize *
            1024 *
            1024
      ) {
        fileUrl &&
          dropzoneElement.value.dropzone.emit('thumbnail', file, fileUrl)
        var thumbnails = file.previewElement.querySelectorAll(
          '[data-dz-thumbnail]'
        )
        thumbnails.forEach((tn: HTMLElement) => {
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

<style lang="scss">
.vue-dropzone {
  border: 2px solid #e5e5e5;
  font-family: 'Arial', sans-serif;
  letter-spacing: 0.2px;
  color: #777;
  transition: 0.2s linear;
  &:hover {
    background-color: #f6f6f6;
  }
  > i {
    color: #ccc;
  }
  > .dz-preview {
    .dz-image {
      border-radius: 0;
      width: 100%;
      height: 100%;
      img {
        &:not([src]) {
          width: 200px;
          height: 200px;
        }
      }
      &:hover {
        img {
          transform: none;
          -webkit-filter: none;
        }
      }
    }
    .dz-details {
      bottom: 0;
      top: 0;
      color: white;
      background-color: rgba(33, 150, 243, 0.8);
      transition: opacity 0.2s linear;
      text-align: left;
      .dz-filename {
        overflow: hidden;
        span {
          background-color: transparent;
        }
        &:not(:hover) {
          span {
            border: none;
          }
        }
        &:hover {
          span {
            background-color: transparent;
            border: none;
          }
        }
      }
      .dz-size {
        span {
          background-color: transparent;
        }
      }
    }
    .dz-progress {
      .dz-upload {
        background: #cccccc;
      }
    }
    .dz-remove {
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
    &:hover {
      .dz-remove {
        opacity: 1;
      }
    }
    .dz-success-mark {
      margin-left: auto;
      margin-top: auto;
      width: 100%;
      top: 35%;
      left: 0;
      svg {
        margin-left: auto;
        margin-right: auto;
      }
    }
    .dz-error-mark {
      margin-left: auto;
      margin-top: auto;
      width: 100%;
      top: 35%;
      left: 0;
      svg {
        margin-left: auto;
        margin-right: auto;
      }
    }
    .dz-error-message {
      margin-left: auto;
      margin-right: auto;
      left: 0;
      width: 100%;
      text-align: center;
      &:after {
        display: none;
      }
    }
    .vdManualThumbnail {
      object-fit: cover;
    }
  }
}

/* Glom */
/*
 * The MIT License
 * Copyright (c) 2012 Matias Meno <m@tias.me>
 */
@keyframes "passing-through" {
  0% {
    opacity: 0;
    transform: translateY(40px);
  }
  30%,
  70% {
    opacity: 1;
    transform: translateY(0px);
  }
  100% {
    opacity: 0;
    transform: translateY(-40px);
  }
}
@keyframes "slide-in" {
  0% {
    opacity: 0;
    transform: translateY(40px);
  }
  30% {
    opacity: 1;
    transform: translateY(0px);
  }
}
@keyframes "pulse" {
  0% {
    transform: scale(1);
  }
  10% {
    transform: scale(1.1);
  }
  20% {
    transform: scale(1);
  }
}
.dropzone {
  box-sizing: border-box;
  min-height: 150px;
  border: 2px solid rgba(0, 0, 0, 0.3);
  background: white;
  padding: 20px 20px;
  * {
    box-sizing: border-box;
  }
  .dz-message {
    text-align: center;
    margin: 2em 0;
  }
  .dz-preview {
    position: relative;
    display: inline-block;
    vertical-align: top;
    margin: 16px;
    min-height: 100px;
    &:hover {
      z-index: 1000;
      .dz-details {
        opacity: 1;
        opacity: 1;
      }
      .dz-image {
        img {
          transform: scale(1.05, 1.05);
          filter: blur(8px);
        }
      }
    }
    .dz-remove {
      font-size: 14px;
      text-align: center;
      display: block;
      cursor: pointer;
      border: none;
      &:hover {
        text-decoration: underline;
      }
    }
    .dz-details {
      z-index: 20;
      position: absolute;
      top: 0;
      left: 0;
      opacity: 0;
      font-size: 13px;
      min-width: 100%;
      max-width: 100%;
      padding: 2em 1em;
      text-align: center;
      color: rgba(0, 0, 0, 0.9);
      line-height: 150%;
      .dz-size {
        margin-bottom: 1em;
        font-size: 16px;
        span {
          background-color: rgba(255, 255, 255, 0.4);
          padding: 0 0.4em;
          border-radius: 3px;
        }
      }
      .dz-filename {
        white-space: nowrap;
        &:hover {
          span {
            border: 1px solid rgba(200, 200, 200, 0.8);
            background-color: rgba(255, 255, 255, 0.8);
          }
        }
        &:not(:hover) {
          overflow: hidden;
          text-overflow: ellipsis;
          span {
            border: 1px solid transparent;
          }
        }
        span {
          background-color: rgba(255, 255, 255, 0.4);
          padding: 0 0.4em;
          border-radius: 3px;
        }
      }
    }
    .dz-image {
      border-radius: 20px;
      overflow: hidden;
      width: 120px;
      height: 120px;
      position: relative;
      display: block;
      z-index: 10;
      img {
        display: block;
      }
    }
    .dz-success-mark {
      pointer-events: none;
      opacity: 0;
      z-index: 500;
      position: absolute;
      display: block;
      top: 50%;
      left: 50%;
      margin-left: -27px;
      margin-top: -27px;
      svg {
        display: block;
        width: 54px;
        height: 54px;
      }
    }
    .dz-error-mark {
      pointer-events: none;
      opacity: 0;
      z-index: 500;
      position: absolute;
      display: block;
      top: 50%;
      left: 50%;
      margin-left: -27px;
      margin-top: -27px;
      svg {
        display: block;
        width: 54px;
        height: 54px;
      }
    }
    &:not(.dz-processing) {
      .dz-progress {
        animation: pulse 6s ease infinite;
      }
    }
    .dz-progress {
      opacity: 1;
      z-index: 1000;
      pointer-events: none;
      position: absolute;
      height: 16px;
      left: 50%;
      top: 50%;
      margin-top: -8px;
      width: 80px;
      margin-left: -40px;
      background: rgba(255, 255, 255, 0.9);
      -webkit-transform: scale(1);
      border-radius: 8px;
      overflow: hidden;
      .dz-upload {
        background: #333;
        background: linear-gradient(to bottom, #666, #444);
        position: absolute;
        top: 0;
        left: 0;
        bottom: 0;
        width: 0;
        transition: width 300ms ease-in-out;
      }
    }
    .dz-error-message {
      pointer-events: none;
      z-index: 1000;
      position: absolute;
      display: block;
      display: none;
      opacity: 0;
      transition: opacity 0.3s ease;
      border-radius: 8px;
      font-size: 13px;
      top: 130px;
      left: -10px;
      width: 140px;
      background: #be2626;
      background: linear-gradient(to bottom, #be2626, #a92222);
      padding: 0.5em 1.2em;
      color: white;
      &:after {
        content: '';
        position: absolute;
        top: -6px;
        left: 64px;
        width: 0;
        height: 0;
        border-left: 6px solid transparent;
        border-right: 6px solid transparent;
        border-bottom: 6px solid #be2626;
      }
    }
  }
  .dz-preview.dz-file-preview {
    .dz-image {
      border-radius: 20px;
      background: #999;
      background: linear-gradient(to bottom, #eee, #ddd);
    }
    .dz-details {
      opacity: 1;
    }
  }
  .dz-preview.dz-image-preview {
    background: white;
    .dz-details {
      transition: opacity 0.2s linear;
    }
  }
  .dz-preview.dz-success {
    .dz-success-mark {
      animation: passing-through 3s cubic-bezier(0.77, 0, 0.175, 1);
    }
  }
  .dz-preview.dz-error {
    .dz-error-mark {
      opacity: 1;
      animation: slide-in 3s cubic-bezier(0.77, 0, 0.175, 1);
    }
    .dz-error-message {
      display: block;
    }
    &:hover {
      .dz-error-message {
        opacity: 1;
        pointer-events: auto;
      }
    }
  }
  .dz-preview.dz-processing {
    .dz-progress {
      opacity: 1;
      transition: all 0.2s linear;
    }
  }
  .dz-preview.dz-complete {
    .dz-progress {
      opacity: 0;
      transition: opacity 0.4s ease-in;
    }
  }
}
.dropzone.dz-clickable {
  cursor: pointer;
  * {
    cursor: default;
  }
  .dz-message {
    cursor: pointer;
    * {
      cursor: pointer;
    }
  }
}
.dropzone.dz-started {
  .dz-message {
    display: none;
  }
}
.dropzone.dz-drag-hover {
  border-style: solid;
  .dz-message {
    opacity: 0.5;
  }
}
.vue-dropzone {
  border: 2px solid #e5e5e5;
  font-family: Arial, sans-serif;
  letter-spacing: 0.2px;
  color: #777;
  transition: 0.2s linear;
  &:hover {
    background-color: #f6f6f6;
  }
  > i {
    color: #ccc;
  }
  > .dz-preview {
    .dz-image {
      border-radius: 0;
      width: 100%;
      height: 100%;
      img {
        &:not([src]) {
          width: 200px;
          height: 200px;
        }
      }
      &:hover {
        img {
          transform: none;
          -webkit-filter: none;
        }
      }
    }
    .dz-details {
      bottom: 0;
      top: 0;
      color: #fff;
      background-color: rgba(33, 150, 243, 0.8);
      transition: opacity 0.2s linear;
      text-align: left;
      .dz-filename {
        overflow: hidden;
        span {
          background-color: transparent;
        }
        &:not(:hover) {
          span {
            border: none;
          }
        }
        &:hover {
          span {
            background-color: transparent;
            border: none;
          }
        }
      }
      .dz-size {
        span {
          background-color: transparent;
        }
      }
    }
    .dz-progress {
      .dz-upload {
        background: #ccc;
      }
    }
    .dz-remove {
      position: absolute;
      z-index: 30;
      color: #fff;
      margin-left: 15px;
      padding: 10px;
      top: inherit;
      bottom: 15px;
      border: 2px #fff solid;
      text-decoration: none;
      text-transform: uppercase;
      font-size: 0.8rem;
      font-weight: 800;
      letter-spacing: 1.1px;
      opacity: 0;
    }
    &:hover {
      .dz-remove {
        opacity: 1;
      }
    }
    .dz-error-mark {
      margin-left: auto;
      margin-top: auto;
      width: 100%;
      top: 35%;
      left: 0;
      svg {
        margin-left: auto;
        margin-right: auto;
      }
    }
    .dz-success-mark {
      margin-left: auto;
      margin-top: auto;
      width: 100%;
      top: 35%;
      left: 0;
      svg {
        margin-left: auto;
        margin-right: auto;
      }
    }
    .dz-error-message {
      margin-left: auto;
      margin-right: auto;
      left: 0;
      width: 100%;
      text-align: center;
      &:after {
        display: none;
      }
    }
  }
}
</style>
