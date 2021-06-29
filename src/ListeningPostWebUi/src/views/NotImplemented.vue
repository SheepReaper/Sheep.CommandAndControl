<template lang="pug">
div
  Jumbotron(header='Sorry!', lead='I had something cool planned for this.')
  p Try one of the other areas...
</template>

<script lang="ts">
import { defineComponent, defineAsyncComponent } from 'vue'

export const NotImplementedView = defineComponent({
  components: {
    Jumbotron: defineAsyncComponent(() => import('../components/Jumbotron.vue'))
  }
})

export { NotImplementedView as default }
</script>
