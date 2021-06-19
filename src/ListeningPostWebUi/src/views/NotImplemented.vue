<template lang="pug">
div
  Jumbotron(header='Sorry!', lead='I had something cool planned for this.')
  p Try one of the other areas...
</template>

<script>
import Jumbotron from '../components/Jumbotron.vue'

export const NotImplementedView = {
  components: { Jumbotron }
}

export { NotImplementedView as default }
</script>
