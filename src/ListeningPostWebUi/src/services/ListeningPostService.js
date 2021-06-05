import Api from './Api'

export default {
  getImplants () {
    return Api.get('/implant')
  }
}
