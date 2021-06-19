<template lang="pug">
#app
  AppBar
  router-view
  AppFooter
</template>

<script>
import { mapActions } from 'vuex'

export const App = {
  components: {
    AppBar: () => import('./components/AppBar.vue'),
    AppFooter: () => import('./components/AppFooter.vue')
  },
  created() {
    this.fetchAgents()
    setInterval(() => this.fetchAgents(), 5000)
  },
  methods: {
    ...mapActions(['fetchAgents'])
  }
}

export { App as default }
</script>

<style lang="scss">
@import '~bootstrap/scss/bootstrap.scss';
</style>
