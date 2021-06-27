<template lang="pug">
nav.navbar.fixed-bottom.navbar-expand.navbar-dark.bg-primary: .container-fluid
  .navbar-brand Listening Station 1.0

  .navbar-nav.me-auto.mb-0: template(v-for='(item, i) in statusItems', :key='i'): .nav-link
    span {{ item.text }}
    span.badge.bg-secondary.ms-1 {{ item.value }}
</template>

<script>
import { useStore } from 'vuex'
import { computed, defineComponent } from 'vue'

export const AppFooter = defineComponent({
  setup: () => {
    const { state } = useStore()

    return {
      statusItems: computed(() => [
        { text: 'Agents Reporting', value: state.agents.length },
        {
          text: 'Pending Tasks',
          value: state.agents.flatMap((a) => a.tasks).length
        },
        {
          text: 'Files Collected',
          value: state.agents.flatMap((a) => a.exfiltratedFiles).length
        }
      ])
    }
  }
})

export { AppFooter as default }
</script>
