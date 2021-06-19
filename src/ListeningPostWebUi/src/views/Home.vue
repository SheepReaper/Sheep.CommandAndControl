<template lang="pug">
div
  Jumbotron(
    header='Dashboard',
    lead='This is where cool graphs and stuff will go'
  )

  p You proably just want to go on to the Command Section....
  router-link.btn.btn-primary.btn-lg(:to='{ path: "/command" }') Begin!
</template>

<script>
import { Jumbotron } from '../components/Jumbotron.vue'

export const HomeView = {
  components: { Jumbotron }
}

export { HomeView as default }
</script>
