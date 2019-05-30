// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import BootstrapVue from 'bootstrap-vue'
import App from './App'
import router from './router'
import CKEditor from '@ckeditor/ckeditor5-vue'

import { Layout, Card } from 'bootstrap-vue/es/components'
import { Scrollspy } from 'bootstrap-vue/es/directives'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import 'vue2-dropzone/dist/vue2Dropzone.min.css'

Vue.config.productionTip = false

Vue.use(BootstrapVue)
Vue.use(Layout)
Vue.use(Card)
Vue.use(Scrollspy)
Vue.use(CKEditor)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
