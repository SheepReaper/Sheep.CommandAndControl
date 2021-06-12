<template lang="pug">
div
  b-jumbotron(
    header='Dashboard',
    lead='This is where cool graphs and stuff will go'
  )
    p You proably just want to go on to the Command Section....
    b-button(:to='{ path: "/command" }', variant='primary') Begin!
</template>

<script>
import { BJumbotron, BButton } from 'bootstrap-vue'
export default {
  name: 'Home',
  components: { BJumbotron, BButton },
  data() {
    return {}
  }
}
</script>
