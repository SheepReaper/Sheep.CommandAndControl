<template lang="pug">
.container
  .row.mt-3: .col-12: QuickCard(
    header='Issue Commands',
    title='Command Dispatcher'
  )
    b-form-group(label='Command Mode:'): b-form-radio-group(
      v-model='commandMode',
      :options='commandOptions'
    )

    b-form-group#commandParameterGroup(
      :invalid-feedback='cPinvalidFeedback',
      :valid-feedback='cPvalidFeedback',
      :state='cPstate',
      description='Please enter the desired bash-compatible command OR SourcePath DestinationPath of a File to Deploy OR The path of a File on target system you wish to retrieve.',
      label='Command / Special Command Arguments',
      label-for='commandParameterInput'
    ): b-form-input#commandParameterInput(
      v-model='commandParameter',
      :state='cPstate',
      trim
    )

    b-form-group#commandParameterGroup2(
      :state='cPstate',
      description='Fair warning, the speed of execution is completely dependant on the rate of check in from agents.',
      label='Looking good? Execute!',
      label-for='submitButton'
    ): button#submitButton.btn.btn-primary(
      :disabled='!cPstate',
      @click='tryCallApi'
    ) SEND IT

  .row.mt-3
    #leftPane.col-6: b-card-group(deck)
      QuickCard.mb-5(
        header='Active Agents (Implants)',
        title='Select which agents will receive commands:'
      )
        b-card-body: b-list-group: b-list-group-item(
          v-for='agent in agents',
          :key='agent.id'
        ): b-form-checkbox(
          v-model='selectedAgents',
          :value='agent.id',
          switch
        )
          b Agent Id: {{ agent.id }}
          b Tasks:
          span.badge {{ agent.tasks.length }}

    #rightPane.col-6: QuickCard(header='Monitor', title='Command History')
      b-card-body: b-list-group: b-list-group-item(
        v-for='task in tasksSorted',
        :key='task.id'
      ) TaskId: {{ task.id }} Command: {{ task.command }}
</template>

<script>
import { mapGetters } from 'vuex'
import { QuickCard } from '../components/QuickCard.vue'
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

import { sendCommand, pullCommand, pushCommand, createAgent } from '../api'

const actionMap = {
  'Debug: Create Agent': createAgent,
  'Direct Bash Command': sendCommand,
  'Special PUSH': pushCommand,
  'Special PULL': pullCommand
}

const commandOptions = Object.keys(actionMap)

const createComparator =
  (key, order = 'asc') =>
  (a, b) => {
    if (
      !Object.prototype.hasOwnProperty.call(a, key) ||
      !Object.prototype.hasOwnProperty.call(b, key)
    ) {
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
  }

export const CommandView = {
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
    BListGroupItem,
    QuickCard
  },
  data: () => ({
    commandParameter: '',
    selectedAgents: [],
    commandMode: '',
    commandOptions
  }),
  computed: {
    ...mapGetters(['agents']),
    tasks() {
      return this.agents.flatMap((a) => a.tasks)
    },
    tasksSorted() {
      return this.tasks.sort(createComparator('id', 'desc'))
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
    tryCallApi() {
      if (this.commandMode in actionMap) {
        actionMap[this.commandMode](this.commandParameter)
          .then((response) => console.log(response))
          .catch((reason) => console.error(['apiError', reason]))
      } else {
        console.error('Invalid commandMode value:', this.commandMode)
      }
    }
  }
}

export { CommandView as default }
</script>
