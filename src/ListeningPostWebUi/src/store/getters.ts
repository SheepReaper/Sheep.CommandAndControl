import { Agent, RootState } from '.'

export const agents = (state: RootState): Agent[] => state.agents

export default {
  agents
}
