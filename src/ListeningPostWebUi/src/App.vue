<template lang="pug">
#app
  AppBar
  router-view
  AppFooter
</template>

<script>
import { defineComponent, defineAsyncComponent } from 'vue'
import { mapActions } from 'vuex'

export const App = defineComponent({
  components: {
    AppBar: defineAsyncComponent(() => import('@/components/AppBar.vue')),
    AppFooter: defineAsyncComponent(() => import('@/components/AppFooter.vue'))
  },
  created() {
    this.fetchAgents()
    setInterval(() => this.fetchAgents(), 5000)
  },
  methods: {
    ...mapActions(['fetchAgents'])
  }
})

export { App as default }
</script>

<style lang="scss">
@import 'bootstrap/scss/bootstrap.scss';
</style>
