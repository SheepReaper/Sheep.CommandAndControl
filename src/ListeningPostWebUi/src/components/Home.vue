<template>
  <div>
    <b-jumbotron header="Dashboard" lead="This is where cool graphs and stuff will go">
      <p>You proably just want to go on to the Command Section....</p>
      <b-button variant="primary" :to="{path: '/command' }">Begin!</b-button>
    </b-jumbotron>
  </div>
</template>

<script>
export default {
  name: 'Home',
  data () {
    return {}
  }
}
</script>

<style lang="scss" scoped>
</style>
