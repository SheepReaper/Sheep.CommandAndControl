<template lang="pug">
b-container(fluid)
  b-row.mt-3: b-col
    | A list of files show up here when available
    b-list-group: b-list-group-item(
      v-for='fileItem in files',
      :key='fileItem.guid'
    )
      | File Name:
      b {{ fileItem.filename }}
      |
      | Guid:
      b {{ fileItem.guid }}
      |
      | Path on Server:
      b {{ fileItem.tempFilePath }}
      |
      | Updated:
      b {{ fileItem.updated }}

  b-row.mt-3: b-col: vue-dropzone#drop1(:options='dropOptions')

  ckeditor(v-model='editorData', :editor='editor', :config='editorConfig')
  b-form-file#inputFile(v-model='pickerFile', @change='loadFile()')
  b-form-textarea#textArea(rows='10')
  button.btn.btn-primary(@click='uploadToServer()') Upload
</template>

<script>
import vue2Dropzone from 'vue2-dropzone'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import CKEditor from '@ckeditor/ckeditor5-vue2'
import {
  BContainer,
  BRow,
  BCol,
  BListGroup,
  BFormFile,
  BButton,
  BListGroupItem,
  BFormTextarea
} from 'bootstrap-vue'
import 'vue2-dropzone/dist/vue2Dropzone.min.css'

import { uploadFile, fetchFiles as _fetchFiles } from '../api'

export const FileReaderView = {
  components: {
    vueDropzone: vue2Dropzone,
    BContainer,
    BRow,
    BCol,
    BFormTextarea,
    BFormFile,
    BButton,
    BListGroup,
    BListGroupItem,
    ckeditor: CKEditor.component
  },
  data: () => ({
    editor: ClassicEditor,
    editorData: '',
    files: [],
    pickerFile: [],
    editorConfig: {},
    dropOptions: {
      url: 'https://localhost:5001/File/upload'
    }
  }),
  computed: {
    endpoint() {
      return this.$parent.endpoint
    }
  },
  mounted() {
    this.fetchFiles()

    setInterval(() => {
      this.fetchFiles()
    }, 5000)
  },
  methods: {
    uploadToServer(file) {
      return uploadFile(file)
        .then(({ data }) => console.log(['uploadResult', data]))
        .catch((reason) => console.error(['uploadApiError', reason]))
    },
    // uploadToServer(file) {
    //   const formData = new FormData()

    //   formData.append('file', file)

    //   fetch(this.endpoint + '/File/pull/0', {
    //     method: 'POST',
    //     mode: 'cors',
    //     headers: {
    //       // 'Content-Type': 'multipart/form-data'
    //       Accept: 'application/json'
    //     },
    //     body: formData
    //   })
    //     .then((response) => response.json())
    //     .then((success) => console.log(success))
    //     .catch((error) => console.log(error))
    // },
    fetchFiles() {
      return _fetchFiles()
        .then(({ data }) => (this.files = data))
        .catch((reason) => console.log(['fetcFilesError', reason]))
    },
    loadFile() {
      const fileToLoad = this.pickerFile.value

      if (fileToLoad) {
        const reader = new FileReader()
        reader.onload = function (fileLoadedEvent) {
          const textFromFileLoaded = fileLoadedEvent.target.result
          document.getElementById('textArea').value = textFromFileLoaded
        }
        reader.readAsText(fileToLoad, 'UTF-8')
      }
    }
  }
}

export { FileReaderView as default }
</script>