import axios from 'axios'
import { retrocycle } from './util'

export const api = axios.create({
  baseURL: 'https://localhost:5001',
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  transformResponse: (data) => retrocycle(JSON.parse(data))
})

export { api as default }

export const fetchAgents = () => api.get('/Implant')

export const createAgent = (id) => api.get(`/Tasking/${id}`)

export const sendCommand = (command, ids) =>
  api.post('/Tasking', { ids, task: { command } })

export const pushCommand = (path, ids) => sendCommand(`push ${path}`, ids)

export const pullCommand = (path, ids) => sendCommand(`pull ${path}`, ids)

export const fetchFiles = () => api.get('/File/')

export const uploadFile = (file) => {
  const formData = new FormData()

  formData.append('file', file)

  return api.post('/File/upload', formData)
}
