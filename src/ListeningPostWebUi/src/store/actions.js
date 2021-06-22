import { fetchAgents as fetchAgentsApi } from '../api'

export const fetchAgents = ({ commit }) =>
  fetchAgentsApi().then(({ data }) => commit('setAgents', data))

export default {
  fetchAgents
}
