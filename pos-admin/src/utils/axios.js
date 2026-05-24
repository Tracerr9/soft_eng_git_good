import axios from 'axios'

// 1. Buat instance Axios dengan URL dasar backend ASP.NET Anda
const api = axios.create({

  baseURL: 'http://localhost:8000', 
})

// 2. Interceptor: Otomatis menyelipkan Token JWT di setiap request
api.interceptors.request.use(
  (config) => {
    // Ambil token dari brankas browser (localStorage)
    const token = localStorage.getItem('jwt_token')
    
    // Jika token ada, pakaikan sebagai gembok Authorization
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

export default api