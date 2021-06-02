<template>
  <b-container fluid>
    <b-row class="mt-3">
      <b-col>
        <b-card-group>
          <b-card
            header-tag="header"
            title="Command Dispatcher"
          >
            <div slot="header">
              <h3>Issue Commands</h3>
            </div>
            <b-card-body>
              <b-form-group label="Command Mode:">
                <b-form-radio-group
                  v-model="commandMode"
                  :options="[
                    'Direct Bash Command',
                    'Special PUSH',
                    'Special PULL',
                    'Debug: Create Agent'
                  ]"
                />
              </b-form-group>
              <b-form-group
                id="commandParameterGroup"
                :invalid-feedback="cPinvalidFeedback"
                :valid-feedback="cPvalidFeedback"
                :state="cPstate"
                description="Please enter the desired bash-compatible command OR SourcePath DestinationPath of a File to Deploy OR The path of a File on target system you wish to retrieve."
                label="Command / Special Command Arguments"
                label-for="commandParameterInput"
              >
                <b-form-input
                  id="commandParameterInput"
                  v-model="commandParameter"
                  :state="cPstate"
                  trim
                />
              </b-form-group>
              <b-form-group
                id="commandParameterGroup2"
                :state="cPstate"
                description="Fair warning, the speed of execution is completely dependant on the rate of check in from agents."
                label="Looking good? Execute!"
                label-for="submitButton"
              >
                <b-button
                  id="submitButton"
                  :disabled="!cPstate"
                  @click="tryCallApi"
                >
                  SEND IT
                </b-button>
              </b-form-group>
            </b-card-body>
          </b-card>
        </b-card-group>
      </b-col>
    </b-row>
    <b-row class="mt-3">
      <b-col id="leftPane">
        <b-card-group deck>
          <b-card
            header-tag="header"
            title="Select which agents will receive commands:"
          >
            <h3 slot="header">Active Agents (Implants)</h3>
            <b-card-body>
              <b-list-group>
                <b-list-group-item
                  v-for="agent in agents"
                  :key="agent.id"
                >
                  <b-form-checkbox
                    :value="agent.id"
                    v-model="selectedAgents"
                    switch
                  >
                    <b>Agent Id: </b>{{ agent.id }} <b>Tasks: </b>
                    <b-badge>{{ agent.tasks.length }} </b-badge>
                  </b-form-checkbox>
                </b-list-group-item>
              </b-list-group>
            </b-card-body>
          </b-card>
          <b-card
            header-tag="header"
            title="Command History"
          >
            <h3 slot="header">Monitor</h3>
            <b-card-body>
              <b-list-group>
                <b-list-group-item
                  v-for="task in tasksSorted"
                  :key="task.id"
                >
                  TaskId: {{ task.id }} Command: {{ task.command }}
                </b-list-group-item>
              </b-list-group>
            </b-card-body>
          </b-card>
        </b-card-group>
      </b-col>
    </b-row>
  </b-container>
</template>
<script>
// import ListeningPostService from '@/services/ListeningPostService'
import {
  BContainer,
  BRow,
  BCol,
  BCardGroup,
  BCard,
  BCardBody,
  BBadge,
  BFormGroup,
  BFormRadioGroup,
  BFormInput,
  BButton,
  BListGroup,
  BListGroupItem,
  BFormCheckbox
} from 'bootstrap-vue'

export default {
  name: 'Command',
  components: {
    BContainer,
    BRow,
    BCol,
    BCardGroup,
    BCard,
    BCardBody,
    BBadge,
    BFormCheckbox,
    BFormGroup,
    BFormRadioGroup,
    BFormInput,
    BButton,
    BListGroup,
    BListGroupItem
  },
  data() {
    return {
      commandParameter: '',
      selectedAgents: [],
      commandMode: ''
    }
  },
  computed: {
    agents() {
      return this.$parent.agents
    },
    tasks() {
      return this.$parent.tasks
    },
    tasksSorted() {
      var newArr = [...this.$parent.tasks].sort(
        this.compareValues('id', 'desc')
      )
      return newArr
    },
    cPstate() {
      return this.commandParameter.length > 0
    },
    cPinvalidFeedback() {
      if (this.commandParameter.length > 0) {
        return ''
      } else {
        return "Please enter something (If you're just spawning an agent, enter an integer that hasn't been used yet.)"
      }
    },
    cPvalidFeedback() {
      return this.cPstate === true ? 'Thanks!' : ''
    }
  },
  methods: {
    compareValues(key, order = 'asc') {
      return function(a, b) {
        if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
          // property doesn't exist on either object
          return 0
        }

        const varA = typeof a[key] === 'string' ? a[key].toUpperCase() : a[key]
        const varB = typeof b[key] === 'string' ? b[key].toUpperCase() : b[key]

        let comparison = 0
        if (varA > varB) {
          comparison = 1
        } else if (varA < varB) {
          comparison = -1
        }

        return order === 'desc' ? comparison * -1 : comparison
        // eslint-disable-next-line
      }
    },
    async tryCallApi() {
      var endpoint = this.$parent.endpoint
      var commandMessage = this.commandParameter
      var param = '/' + commandMessage
      var controller = '/Tasking'

      var messageBody = {
        command: commandMessage
      }

      var requestOptions = {
        method: 'POST',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json'
        }
      }

      if (this.commandMode === 'Debug: Create Agent') {
        requestOptions['method'] = 'GET'
      } else if (this.commandMode === 'Direct Bash Command') {
        requestOptions['body'] = JSON.stringify(messageBody)
        param = ''
      } else if (this.commandMode === 'Special PUSH') {
        param = ''
        messageBody['command'] = 'push ' + messageBody['command']
        requestOptions['body'] = JSON.stringify(messageBody)
      } else if (this.commandMode === 'Special PULL') {
        param = ''
        messageBody['command'] = 'pull ' + messageBody['command']
        requestOptions['body'] = JSON.stringify(messageBody)
      } else {
        return
      }

      // else {
      //   this.commandMode === 'Direct Bash Command' {
      //   (this.commandMode === 'Special PUSH' || this.commandMode === 'Special PULL') {
      //   controller = '/File'
      // }

      // const messageContent = {
      //   command: param
      // }

      var apiString = endpoint + controller + param

      fetch(apiString, requestOptions)
        .then(response => response.json())
        .then(data => {
          console.log(data)
        })
        .catch(error => console.error(error))
    }
  }
}
</script>
<style lang="scss" scoped></style>
