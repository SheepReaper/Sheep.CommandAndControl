// import Vue from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

export const router = createRouter({
  history: createWebHistory(),
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
      path: '/fileManager',
      name: 'FileManager',
      component: () => import('./views/FileReader.vue')
    },
    {
      path: '/docs',
      name: 'Docs',
      component: () => import('./views/ApiDocs.vue')
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: () => import('./views/NotImplemented.vue')
    }
  ]
})

export { router as default }
