<template>
  <div>
    <b-jumbotron
      header="Dashboard"
      lead="This is where cool graphs and stuff will go"
    >
      <p>You proably just want to go on to the Command Section....</p>
      <b-button
        :to="{ path: '/command' }"
        variant="primary"
      >Begin!</b-button>
    </b-jumbotron>
  </div>
</template>

<script>
import { BJumbotron, BButton } from 'bootstrap-vue'
export default {
  name: 'Home',
  components: { BJumbotron, BButton },
  data() {
    return {}
  }
}
</script>

<style lang="scss" scoped></style>
