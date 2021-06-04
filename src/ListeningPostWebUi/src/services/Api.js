import axios from 'axios'

const api = axios.create({
  baseURL: 'https://localhost:5001',
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  }
})

export default api
