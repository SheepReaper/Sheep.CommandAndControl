<template lang="pug">
.container
  .row.mt-3: .col
    label.form-label(for='fileList') A list of files show up here when available
    ul#fileList.list-group: template(
      v-for='({ filename, guid, tempFilePath, updated }, i) in files',
      :key='i'
    ): a.list-group-item.list-group-item-action(
      :href='`https://localhost:5001/file/download/${guid}`'
    )
      span File Name:
      b.ms-1 {{ filename }}
      span.ms-2 Guid:
      b.ms-1 {{ guid }}
      span.ms-2 Path on Server:
      b.ms-1 {{ tempFilePath }}
      span.ms-2 Updated:
      b.ms-1 {{ updated }}

  .row.mt-3: .col
    label.form-text(for='fileUploader') Upload files directly
    vdrop#fileUploader(:options='otherOptions' :useCustomSlot="true")

  .row.mt-3
    .col-12
      label.form-text(for='fileEditor') Create or edit a file
      ckeditor#fileEditor(
        v-model='editorData',
        :editor='editor',
        :config='editorConfig'
      )
    .col-12.mt-3
      label.form-text(for='inputFile') Load a file to edit
      input#inputFile.form-control(type='file', @change='onPickFile')

  .row.mt-3: .col: button.btn.btn-primary(
    :disabled='editorData === ""',
    @click='onSave'
  ) Save &amp; Upload
</template>

<script>
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'

import {
  defineComponent,
  defineAsyncComponent,
  reactive,
  onMounted,
  ref
} from 'vue'

import { uploadFiles, fetchFiles } from '../api'

/**
 * @param {File[]} acceptFiles
 * @param {object[]} rejectFiles
 * @param {File} rejectFiles.file
 * @param {object[]} rejectFiles.errors
 * @param {string} rejectFiles.errors.code
 * @param {string} rejectFiles.errors.message
 */
const onDrop = (acceptFiles, rejectFiles) => {
  if (acceptFiles.length > 0) uploadFiles(acceptFiles)

  rejectFiles.map((rf) => console.error(['dropFileError', rf]))
}

export const FileReaderView = defineComponent({
  components: {
    vdrop: defineAsyncComponent(() => import('@/components/VueDropZone.vue')),
    ckeditor: defineAsyncComponent(() =>
      import('@ckeditor/ckeditor5-vue').then((module) => module.component)
    )
  },
  setup: () => {
    const files = ref([])
    const editorData = ref('')
    const editorConfig = ref({})
    const currentFile = ref(new File([], ''))
    const otherOptions = ref({ url: 'https://localhost:5001/File' })

    const onOpen = () => _open()

    /**
     * @param {object} param
     * @param {HTMLInputElement} param.target
     */
    const onPickFile = ({ target: { files } }) => {
      currentFile.value = files[0]

      files[0]
        .text()
        .then((text) => (editorData.value = text))
        .catch((reason) => console.error(['filePickerError', reason]))
    }

    const onSave = () => {
      const options = { type: currentFile.value.type }

      return uploadFiles([
        new File(
          [new Blob([editorData.value], options)],
          currentFile.value.name,
          options
        )
      ])
    }

    const updateFileList = () =>
      fetchFiles().then(({ data }) => (files.value = data))

    setInterval(() => {
      updateFileList()
    }, 5000)

    onMounted(() => {
      updateFileList()
    })

    return {
      editor: ClassicEditor,
      editorConfig,
      editorData,
      files,
      onPickFile,
      onSave,
      otherOptions
    }
  }
})

export { FileReaderView as default }
</script>
