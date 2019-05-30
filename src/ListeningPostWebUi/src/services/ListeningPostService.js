import Api from '@/services/Api'

export default {
  getImplants () {
    return Api.get('/implant')
  }
}
