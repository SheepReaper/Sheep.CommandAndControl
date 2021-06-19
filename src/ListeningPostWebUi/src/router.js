import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export const router = new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: () => import('./views/Home.vue')
    },
    {
      path: '/command',
      name: 'Command',
      component: () => import('./views/Command.vue')
    },
    {
      path: '/queues',
      name: 'Queues',
      component: () => import('./views/NotImplemented.vue')
    },
    {
      path: '/fileManager',
      name: 'FileManager',
      component: () => import('./views/FileReader.vue')
    },
    {
      path: '/docs',
      name: 'Docs',
      component: () => import('./views/ApiDocs.vue')
    }
  ]
})

export { router as default }
