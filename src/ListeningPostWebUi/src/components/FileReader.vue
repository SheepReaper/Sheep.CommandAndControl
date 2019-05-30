<template>
  <b-container fluid>
    <b-row class="mt-3">
      <b-col>
        A list of files show up here when available
        <b-list-group>
          <b-list-group-item
            v-for="fileItem in this.files"
            :key="fileItem.guid"
          >
          File Name: <b>{{fileItem.filename}}</b> Guid: <b>{{fileItem.guid}}</b> Path on Server: <b>{{fileItem.tempFilePath}}</b> Updated: <b>{{fileItem.updated}}</b>
          </b-list-group-item>
        </b-list-group>
      </b-col>
    </b-row>
    <b-row class="mt-3">
      <b-col>
        <vue-dropzone
          id="drop1"
          :options="dropOptions"
          @vdropzone-complete="uploadToServer"
        ></vue-dropzone>
        <ckeditor :editor="editor" v-model="editorData" :config="editorConfig"></ckeditor>
        <b-form-file id="inputFile" v-on:change="loadFile()" v-model="pickerFile"></b-form-file>
        <b-form-textarea rows="10" id="textArea"></b-form-textarea>
        <b-button @click="uploadToServer()">Upload</b-button>
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
import vueDropzone from 'vue2-dropzone'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'

export default {
  name: 'FileReader',
  data () {
    return {
      editor: ClassicEditor,
      editorData: '',
      files: [],
      pickerFile: [],
      editorConfig: {
      },
      dropOptions: {
        url: 'https://localhost:5001/File/pull/0'
      }
    }
  },
  computed: {
    endpoint () {
      return this.$parent.endpoint
    }
  },
  components: {
    vueDropzone
  },
  methods: {
    uploadToServer (file) {
      let formData = new FormData()

      formData.append('file', file)

      fetch(this.endpoint + '/File/pull/0', {
        method: 'POST',
        mode: 'cors',
        headers: {
          // 'Content-Type': 'multipart/form-data'
          Accept: 'application/json'
        },
        body: formData
      })
        .then(response => response.json())
        .then(success => console.log(success))
        .catch(error => console.log(error))
    },
    fetchFiles () {
      fetch(this.endpoint + '/File/', {
        method: 'GET',
        mode: 'cors',
        headers: {
          Accept: 'application/json'
        }
      })
        .then(response => response.json())
        .then(success => {
          this.files = success
          console.log(success)
        })
        .catch(error => console.log(error))
    },
    loadFile () {
      var fileToLoad = this.pickerFile.value

      if (fileToLoad) {
        var reader = new FileReader()
        reader.onload = function (fileLoadedEvent) {
          var textFromFileLoaded = fileLoadedEvent.target.result
          document.getElementById('textArea').value = textFromFileLoaded
        }
        reader.readAsText(fileToLoad, 'UTF-8')
      }
    }
  },
  mounted () {
    this.fetchFiles()

    setInterval(() => {
      this.fetchFiles()
    }, 5000)
  }
}
</script>
