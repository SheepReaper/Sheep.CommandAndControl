import { fetchAgents as fetchAgentsApi } from '../api'

export const fetchAgents = ({ commit }) =>
  fetchAgentsApi()
    .then(({ data }) => commit('setAgents', data))
    .catch((reason) => console.error(['apiError', reason]))

export default {
  fetchAgents
}
