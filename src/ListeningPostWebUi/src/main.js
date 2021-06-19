import Vue from 'vue'
import { router } from './router'
import { store } from './store'

// import 'vue2-dropzone/dist/vue2Dropzone.min.css'

const App = () => import('./App.vue')

Vue.config.productionTip = false

/* eslint-disable no-new */
new Vue({
  router,
  store,
  render: (h) => h(App)
}).$mount('#app')
