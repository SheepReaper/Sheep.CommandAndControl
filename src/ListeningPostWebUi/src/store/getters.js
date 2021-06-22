export const agents = (state) => {
  if (state.agents === undefined) {
    return []
  }
  return state.agents
}

export default {
  agents
}
