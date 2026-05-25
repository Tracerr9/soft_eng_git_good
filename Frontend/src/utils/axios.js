import axios from 'axios'

const api = axios.create({
  // Masukkan URL publik dari teman Anda di sini
  baseURL: 'http://103.82.240.234:9032', 
  
  headers: {
    'Content-Type': 'application/json'
  }
})

// Otomatis selipkan Token JWT di setiap request jika sudah login
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('jwt_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default api