<template lang="pug">
div
  Jumbotron(
    header='Dashboard',
    lead='This is where cool graphs and stuff will go'
  )

  p You proably just want to go on to the Command Section....
  router-link.btn.btn-primary.btn-lg(:to='{ path: "/command" }') Begin!
</template>

<script lang="ts">
import { defineComponent, defineAsyncComponent } from 'vue'

export const HomeView = defineComponent({
  components: {
    Jumbotron: defineAsyncComponent(() => import('../components/Jumbotron.vue'))
  }
})

export { HomeView as default }
</script>
