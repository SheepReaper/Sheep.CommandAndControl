<template lang="pug">
.container
  .row.mt-3: .col
    label.form-label(for='fileList') A list of files show up here when available
    ul#fileList.list-group: template(
      v-for='({ filename, guid, tempFilePath, updated }, i) in files'
    ): a.list-group-item.list-group-item-action(
      :key='i',
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
    vue-dropzone#fileUploader(:options='dropOptions')

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
import vue2Dropzone from 'vue2-dropzone'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import CKEditor from '@ckeditor/ckeditor5-vue2'

import { uploadFile, fetchFiles as _fetchFiles } from '../api'

export const FileReaderView = {
  components: {
    vueDropzone: vue2Dropzone,
    ckeditor: CKEditor.component
  },
  data: () => ({
    editor: ClassicEditor,
    editorData: '',
    files: [],
    currentFile: {},
    editorConfig: {},
    dropOptions: {
      url: 'https://localhost:5001/File/upload'
    }
  }),
  mounted() {
    this.fetchFiles()

    setInterval(() => {
      this.fetchFiles()
    }, 5000)
  },
  methods: {
    onSave() {
      return uploadFile(
        new File(
          [
            new Blob([this.editorData], {
              type: this.currentFile.type
            })
          ],
          this.currentFile.name,
          {
            type: this.currentFile.type
          }
        )
      ).catch((reason) => console.error(['fileUploadError', reason]))
    },
    onPickFile({ target: { files } }) {
      this.currentFile = files[0]
      files[0]
        .text()
        .then((text) => (this.editorData = text))
        .catch((reason) => console.error(['readFileError', reason]))
    },
    fetchFiles() {
      return _fetchFiles()
        .then(({ data }) => (this.files = data))
        .catch((reason) => console.log(['fetcFilesError', reason]))
    }
  }
}

export { FileReaderView as default }
</script>

<style lang="scss" scoped>
@import '~vue2-dropzone/dist/vue2Dropzone.min.css';
</style>
