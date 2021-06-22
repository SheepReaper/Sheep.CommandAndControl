<template lang="pug">
.card
  h2.card-header {{ header }}
  .card-body
    h5.card-title {{ title }}
    slot
</template>

<script>
import { defineComponent } from 'vue'
export const QuickCard = defineComponent({
  props: {
    header: {
      type: String,
      required: false,
      default: ''
    },
    title: {
      type: String,
      required: false,
      default: ''
    }
  }
})

export { QuickCard as default }
</script>
