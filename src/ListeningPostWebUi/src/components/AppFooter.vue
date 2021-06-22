<template lang="pug">
nav.navbar.fixed-bottom.navbar-expand.navbar-dark.bg-primary: .container-fluid
  .navbar-brand Listening Station 1.0

  .navbar-nav.me-auto.mb-0: template(v-for='(item, i) in statusItems' :key='i'): .nav-link
    span {{ item.text }}
    span.badge.bg-secondary.ms-1 {{ item.value }}
</template>

<script>
import { mapGetters } from 'vuex'
import { defineComponent } from 'vue'

export const AppFooter = defineComponent({
  data: () => ({
    logo: { text: 'C & C', route: { path: '/' } },
    links: [
      { text: 'Home', route: { path: '/' } },
      { text: 'Command', route: { path: '/command' } },
      { text: 'Manage Queues', route: { path: '/queues' } },
      { text: 'File Manager', route: { path: '/fileManager' } },
      { text: 'Api Docs', route: { path: '/docs' } }
    ]
  }),
  computed: {
    ...mapGetters(['agents']),
    statusItems() {
      return [
        { text: 'Agents Reporting', value: this.agents.length },
        {
          text: 'Pending Tasks',
          value: this.agents.flatMap((a) => a.tasks).length
        },
        {
          text: 'Files Collected',
          value: this.agents.flatMap((a) => a.exfiltratedFiles).length
        }
      ]
    }
  }
})

export { AppFooter as default }
</script>
