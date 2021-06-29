import { createStore } from 'vuex'

import getters from './getters'
import actions from './actions'
import mutations from './mutations'

interface Task {
  id: number
}

interface ExfiltratedFile {
  id: number
}

export interface Agent {
  tasks: Task[]
  exfiltratedFiles: ExfiltratedFile[]
}

export interface RootState {
  agents: Agent[]
}

const state: RootState = {
  agents: []
}

export const store = createStore<RootState>({
  state,
  getters,
  actions,
  mutations
})

export { store as default }
