import { createApp, defineAsyncComponent } from 'vue'
import { router } from './router'
import { store } from './store'

const app = defineAsyncComponent(() => import('./App.vue'))

createApp(app).use(store).use(router).mount('#app')
