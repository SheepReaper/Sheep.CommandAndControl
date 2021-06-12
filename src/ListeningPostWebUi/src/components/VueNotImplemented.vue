<template lang="pug">
div
  b-jumbotron(header='Sorry!', lead='I had something cool planned for this.')
    p Try one of the other areas...
</template>

<script>
import { BJumbotron } from 'bootstrap-vue'
export default {
  name: 'VueNotImplemented',
  components: { BJumbotron },
  data() {
    return {}
  }
}
</script>
