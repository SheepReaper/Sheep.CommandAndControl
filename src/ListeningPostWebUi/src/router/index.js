import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/components/Home'
import Command from '@/components/Command'
import ApiDocs from '@/components/ApiDocs'
import FileReader from '@/components/FileReader'
import VueNotImplemented from '@/components/VueNotImplemented'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/command',
      name: 'Command',
      component: Command
    },
    {
      path: '/queues',
      name: 'Queues',
      component: VueNotImplemented
    },
    {
      path: '/fileManager',
      name: 'FileManager',
      component: FileReader
    },
    {
      path: '/docs',
      name: 'Docs',
      component: ApiDocs
    }
  ]
})
