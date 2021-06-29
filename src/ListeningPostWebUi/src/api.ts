import axios, { AxiosResponse } from 'axios'
import { Agent } from './store'
import { JsonNetDecycle } from './util'

const processSTJ = JsonNetDecycle.retrocycle

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

export const fetchAgents = (): Promise<AxiosResponse<Agent[]>> =>
  api.get('/Implant')

export const createAgent = (
  id: string | number
): Promise<AxiosResponse<unknown>> => api.get(`/Tasking/${id}`)

export const sendCommand = (
  command: string,
  ids: number[]
): Promise<AxiosResponse<unknown>> =>
  api.post('/Tasking', { ids, task: { command } })

export const pushCommand = (
  path: string,
  ids: number[]
): Promise<AxiosResponse<unknown>> => sendCommand(`push ${path}`, ids)

export const pullCommand = (
  path: string,
  ids: number[]
): Promise<AxiosResponse<unknown>> => sendCommand(`pull ${path}`, ids)

interface FileDto {
  filename: string
  guid: string
  tempFilePath: string
  updated: Date
}

export const fetchFiles = (): Promise<AxiosResponse<FileDto[]>> =>
  api.get('/File/')

export const uploadFiles = (files: File[]): Promise<AxiosResponse<unknown>> => {
  const formData = new FormData()

  files.map((f) => formData.append('file', f, f.name))

  return api.post('/File', formData)
}
