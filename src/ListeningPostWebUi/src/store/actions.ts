import { ActionContext } from 'vuex'
import { RootState } from '.'
import { fetchAgents as fetchAgentsApi } from '../api'

export const fetchAgents = ({
  commit
}: ActionContext<RootState, RootState>): Promise<void> =>
  fetchAgentsApi().then(({ data }) => commit('setAgents', data))

export default {
  fetchAgents
}
