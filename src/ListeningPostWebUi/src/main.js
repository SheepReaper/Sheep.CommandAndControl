// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import App, { mountPoint } from './App.vue'
import router from './router'
// import CKEditor from '@ckeditor/ckeditor5-vue'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import 'vue2-dropzone/dist/vue2Dropzone.min.css'

Vue.config.productionTip = false

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
// Vue.use(LayoutPlugin)
// Vue.use(CardPlugin)
// Vue.use(VBScrollspyPlugin)
// Vue.use(CKEditor)

/* eslint-disable no-new */
new Vue({
  el: mountPoint,
  router,
  components: { App },
  template: '<App/>'
})
