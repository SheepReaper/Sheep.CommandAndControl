import axios from 'axios'
import { retrocycle as processSTJ } from './util'

const api = axios.create({
  baseURL: 'https://localhost:5001',
  withCredentials: false
})

api.interceptors.response.use(
  (response) => {
    response.data = processSTJ(response.data)
    return response
  },
  (error) => {
    console.error(['axiosApiErrorError', error])
    return Promise.reject(error)
  }
)

export { api, api as default }

export const fetchAgents = () => api.get('/Implant')

export const createAgent = (/** @type {string | number} */ id) =>
  api.get(`/Tasking/${id}`)

export const sendCommand = (
  /** @type {string} */ command,
  /** @type {number[]} */ ids
) => api.post('/Tasking', { ids, task: { command } })

export const pushCommand = (
  /** @type {string} */ path,
  /** @type {number[]} */ ids
) => sendCommand(`push ${path}`, ids)

export const pullCommand = (
  /** @type {string} */ path,
  /** @type {number[]} */ ids
) => sendCommand(`pull ${path}`, ids)

/**
 * @typedef {object} FileDto
 * @property {string} filename
 * @property {string} guid
 * @property {string} tempFilePath
 * @property {Date} updated
 */

/** @returns {Promise<import('axios').AxiosResponse<FileDto[]>>} */
export const fetchFiles = () => api.get('/File/')

export const uploadFiles = (/** @type {File[]} */ files) => {
  const formData = new FormData()

  files.map((f) => formData.append('file', f, f.name))

  return api.post('/File', formData)
}
