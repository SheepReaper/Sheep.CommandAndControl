<template lang="pug">
label.text-reader
  | Read File
  input(type='file', @change='loadTextFromFile')
</template>

<script>
import { defineComponent } from 'vue'

export const TextReader = defineComponent({
  methods: {
    loadTextFromFile(ev) {
      const file = ev.target.files[0]
      const reader = new FileReader()

      reader.onload = (e) => this.$emit('load', e.target.result)
      reader.readAsText(file)
    }
  }
})

export { TextReader as default }
</script>

<style scoped>
.text-reader {
  position: relative;
  overflow: hidden;
  display: inline-block;

  border: 2px solid black;
  border-radius: 5px;
  padding: 8px 12px;
  cursor: pointer;
}
.text-reader input {
  position: absolute;
  top: 0;
  left: 0;
  z-index: -1;
  opacity: 0;
}
</style>
