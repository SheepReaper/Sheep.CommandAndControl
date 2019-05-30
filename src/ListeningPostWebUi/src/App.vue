<template>
  <div id="app">
    <b-navbar toggleable="md" type="dark" variant="primary" sticky>
      <b-navbar-brand href="#">C &amp; C</b-navbar-brand>

      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <b-nav-item :to="{ path: '/' }">Home</b-nav-item>
          <b-nav-item :to="{ path: '/command' }">Command</b-nav-item>
          <b-nav-item :to="{ path: '/queues' }">Manage Queues</b-nav-item>
          <b-nav-item :to="{ path: '/fileManager' }">File Manager</b-nav-item>
          <b-nav-item :to="{ path: '/docs' }">Api Docs</b-nav-item>
        </b-navbar-nav>

        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <b-nav-form>
            <b-form-input
              size="sm"
              class="mr-sm-2"
              placeholder="Global Search (Not Implemented)"
            ></b-form-input>
            <b-button size="sm" class="my-2 my-sm-0" type="submit"
              >Search</b-button
            >
          </b-nav-form>

          <!-- <b-nav-item-dropdown right>
          <template slot="button-content"><em>User</em></template>
          <b-dropdown-item href="#">Profile</b-dropdown-item>
          <b-dropdown-item href="#">Sign Out</b-dropdown-item>
        </b-nav-item-dropdown> -->
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <router-view />

    <b-navbar fixed="bottom" toggleable="sm" type="dark" variant="primary">
      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <b-navbar-brand>Listening Station 1.0</b-navbar-brand>
          <b-nav-item
            >Agents Reporting <b-badge>{{ agentCount }}</b-badge></b-nav-item
          >
          <b-nav-item
            >Pending Tasks <b-badge>{{ pendingTaskCount }}</b-badge></b-nav-item
          >
          <b-nav-item
            >Files Collected
            <b-badge>{{ exfiltratedFileCount }}</b-badge></b-nav-item
          >
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
  </div>
</template>

<script>
// import ListeningPostService from '@/services/ListeningPostService'

export default {
  name: 'App',
  props: {
    endpoint: {
      type: String,
      default: 'https://localhost:5001'
    }
  },
  data () {
    return {
      agents: [],
      tasks: [],
      files: []
    }
  },
  computed: {
    agentCount () {
      return this.agents.length
    },
    pendingTaskCount () {
      return this.tasks.length
    },
    exfiltratedFileCount () {
      return this.files.length
    }
  },
  methods: {
    async fetchAgents () {
      fetch(this.endpoint + '/implant')
        .then(response => response.json())
        .then(data => {
          this.agents = data
        })
        .catch(error => console.error(error))
    },
    async updateTasks () {
      var newTasks = []
      for (var i = 0; i < this.agents.length; i++) {
        var agent = this.agents[i]
        for (var j = 0; j < agent.tasks.length; j++) {
          var task = agent.tasks[j]
          newTasks.push(task)
        }
      }
      this.tasks = newTasks
    },
    async updateFiles () {
      var newFiles = []
      for (var i = 0; i < this.agents.length; i++) {
        var agent = this.agents[i]
        for (var j = 0; j < agent.exfiltratedFiles.length; j++) {
          var file = agent.exfiltratedFiles[j]
          newFiles.push(file)
        }
      }
      this.files = newFiles
    }
  },
  mounted () {
    this.fetchAgents()

    setInterval(() => this.fetchAgents(), 5000)
    setInterval(() => this.updateTasks(), 5000)
    setInterval(() => this.updateFiles(), 5000)
  }
}
</script>

<style lang="scss">
</style>
