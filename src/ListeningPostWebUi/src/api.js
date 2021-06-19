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

export const sendCommand = (command) => api.post('/Tasking', { command })

export const pushCommand = (path) => sendCommand(`push ${path}`)

export const pullCommand = (path) => sendCommand(`pull ${path}`)

export const fetchFiles = () => api.get('/File/')

export const uploadFile = (file) => {
  const formData = new FormData()

  formData.append('file', file)

  return api.post('/File/upload', formData)
}
