<template lang="pug">
nav.navbar.navbar-expand-lg.navbar-dark.bg-primary: .container-fluid
  button.navbar-toggler(
    type='button',
    data-bs-toggle='collapse',
    data-bs-target='#collapseTarget',
    aria-controls='collapseTarget',
    aria-expanded='false',
    aria-label='Toggle navigation'
  ): span.navbar-toggler-icon

  router-link.navbar-brand(:to='logo.route') {{ logo.text }}

  #collapseTarget.collapse.navbar-collapse
    .navbar-nav.me-auto.mb-2.mb-lg-0: template(
      v-for='(link, i) in links',
      :key='i'
    ): router-link.nav-link(
      :class='{ active: isCurrentRoute(link.route) }',
      :to='link.route',
      :aria-current='isCurrentRoute(link.route) ? "page" : false'
    ) {{ link.text }}

    form.d-flex
      input.form-control.me-2(
        type='search',
        placeholder='Search (coming soon)',
        aria-label='Search'
      )

      button.btn.btn-outline-light(type='submit') Search
</template>

<script>
import { defineComponent } from 'vue'

export const AppBar = defineComponent({
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
  methods: {
    isCurrentRoute(route) {
      return route.name === this.$route.name || route.path === this.$route.path
    }
  }
})

export { AppBar as default }
</script>
