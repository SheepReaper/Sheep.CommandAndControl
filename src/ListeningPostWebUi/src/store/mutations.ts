import { Agent } from '.'
import { RootState } from '.'

export const setAgents = (state: RootState, value: Agent[]): void => {
  state.agents = value
}

export default {
  setAgents
}
