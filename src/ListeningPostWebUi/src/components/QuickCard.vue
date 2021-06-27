<template lang="pug">
.card
  h2.card-header(v-if="header") {{ header }}
  .card-body
    h5.card-title(v-if="title") {{ title }}
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
