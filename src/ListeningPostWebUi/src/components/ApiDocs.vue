<template>
    <div id="iframe-wrapper" :style="iframe.wrapperStyle">
        <iframe v-if="loaded"
        :src="iframe.src"
        :style="iframe.style"
        :height="iframe.style.height"
        :width="iframe.style.width"
        type="application/pdf"
        frameborder="0"
      ></iframe>
    </div>
</template>
<script>
export default {
  data () {
    return {
      loaded: false,
      iframe: {
        // Replace with shorter url if iframe is allowed to redirect
        // src: 'https://localhost:8080/swagger',
        src: 'https://localhost:5001/swagger/index.html',
        style: null,
        wrapperStyle: null
      }
    }
  },
  mounted () {
    this.iframe.style = {
      // position: 'absolute',
      width: window.innerWidth,
      height: window.innerHeight
    }
    this.iframe.wrapperStyle = {
      overflow: 'hidden'
    }
    this.loaded = true
  }
}
</script>
