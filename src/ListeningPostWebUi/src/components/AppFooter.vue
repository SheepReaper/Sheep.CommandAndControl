<template lang="pug">
nav.navbar.fixed-bottom.navbar-expand.navbar-dark.bg-primary: .container-fluid
  .navbar-brand Listening Station 1.0

  .navbar-nav.me-auto.mb-0: template(
    v-for='({ text, value }, i) in statusItems',
    :key='i'
  ): .nav-link
    span(v-text='text')
    span.badge.bg-secondary.ms-1(v-text='value')
</template>

<script lang="ts">
import { useStore } from 'vuex'
import { computed, defineComponent } from 'vue'
import { RootState } from '@/store'

export const AppFooter = defineComponent({
  setup: () => {
    const { state } = useStore<RootState>()

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
