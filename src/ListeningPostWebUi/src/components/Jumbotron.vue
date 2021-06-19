<template lang="pug">
.p-5.mb-4.bg-light.rounded-3
  .container-fluid.py-5
    h1.display-5.fw-bold {{ header }}
    p.col-md-8.fs-4 {{ lead }}
</template>

<script>
export const Jumbotron = {
  props: {
    header: {
      type: String,
      required: false,
      default: ''
    },
    lead: {
      type: String,
      required: false,
      default: ''
    }
  }
}

export { Jumbotron as default }
</script>
