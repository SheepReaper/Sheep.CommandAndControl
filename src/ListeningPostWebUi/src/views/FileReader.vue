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
      b.ms-1(v-text='filename')
      span.ms-2 Guid:
      b.ms-1(v-text='guid')
      span.ms-2 Path on Server:
      b.ms-1(v-text='tempFilePath')
      span.ms-2 Updated:
      b.ms-1(v-text='updated')

  .row.mt-3: .col
    label.form-text(for='fileUploader') Upload files directly
    vue-dropzone#fileUploader(:options='dropzoneOptions')

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

<script lang="ts">
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'

import { defineComponent, defineAsyncComponent, onMounted, ref } from 'vue'

import { uploadFiles, fetchFiles } from '../api'

export const FileReaderView = defineComponent({
  components: {
    vueDropzone: defineAsyncComponent(() => import('@/components/VueDropzone')),
    ckeditor: defineAsyncComponent(() =>
      import('@ckeditor/ckeditor5-vue').then((module) => module.component)
    )
  },
  setup: () => {
    const files = ref([])
    const editorData = ref('')
    const editorConfig = ref({})
    const currentFile = ref(new File([], ''))
    const dropzoneOptions = ref({
      url: 'https://localhost:5001/File'
      // url: 'https://httpbin.org/post'
    })

    const onPickFile = ({
      target: { files }
    }: {
      target: HTMLInputElement
    }) => {
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
      dropzoneOptions
    }
  }
})

export { FileReaderView as default }
</script>
