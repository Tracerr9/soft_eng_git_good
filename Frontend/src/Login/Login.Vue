<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/axios'

const router = useRouter()

const formLogin = ref({
  username: '',
  password: ''
})

const prosesLogin = async () => {
  try {
    const response = await api.post('/api/login', {
      username: formLogin.value.username,
      password: formLogin.value.password
    })

    // KITA CEK BENTUK ASLI DARI BACKEND DI CONSOLE
    console.log("Data dari backend:", response.data)

    // AMBIL TOKEN DENGAN SEGALA KEMUNGKINAN HURUF BESAR/KECIL
    const token = response.data.token || response.data.Token || response.data.jwtToken
    
    if (!token) {
       alert("Error: Backend tidak mengembalikan Token!")
       return // Hentikan proses jika token tidak ada
    }

    // Simpan token yang benar ke Local Storage
    localStorage.setItem('jwt_token', token)
    
    // (Opsional) Simpan data role/username
    localStorage.setItem('user_role', response.data.role || response.data.Role || 'Admin')
    localStorage.setItem('username', response.data.username || response.data.Username || formLogin.value.username)

    alert('Berhasil Login dari API Backend!')
    router.push('/admin-dashboard')

  } catch (error) {
    console.error("Error dari backend:", error)
    alert('Login Gagal! Pastikan Username/Password benar.')
  }
}
</script>

<template>
  <div class="d-flex justify-content-center align-items-center bg-light" style="min-height: 100vh;">
    <div class="card shadow-sm p-4" style="width: 400px;">
      <h3 class="text-center mb-4 fw-bold">Smart POS Login</h3>
      
      <form @submit.prevent="prosesLogin">
        <div class="mb-3">
          <label class="form-label">Username / Email</label>
          <input type="text" class="form-control" v-model="formLogin.username" placeholder="Masukkan username">
        </div>
        
        <div class="mb-4">
          <label class="form-label">Password</label>
          <input type="password" class="form-control" v-model="formLogin.password" placeholder="Masukkan password">
        </div>
        
        <button type="submit" class="btn btn-primary w-100 fw-bold">Login</button>
      </form>
      
    </div>
  </div>
</template>