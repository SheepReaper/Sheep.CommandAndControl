import axios from 'axios'

export default () => {
  return axios.create({
    baseURL: `https://localhost:5001`,
    withCredentials: false,
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    }
  })
}
