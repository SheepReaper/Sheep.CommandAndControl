import { createStore } from 'vuex'

import getters from './getters'
import actions from './actions'
import mutations from './mutations'

// const state = () => ({agents: []})

const state = {
  agents: []
}

export const store = createStore({
  state,
  getters,
  actions,
  mutations
})

// if (module.hot) {
//   module.hot.accept(['./mutations'], () => {
//     store.hotUpdate({
//       mutations: require('./mutations').default
//     })
//   })
// }

export { store as default }
