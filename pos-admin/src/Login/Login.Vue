<!-- <script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/axios' // Memanggil kurir Axios yang sudah kita setting

const router = useRouter()

// Variabel penampung input (Pastikan v-model di HTML Anda cocok dengan ini)
const formLogin = ref({
  username: '',
  password: ''
})

const prosesLogin = async () => {
  try {
    // 1. Axios menembak API backend
    const response = await api.post('/api/login', {
      username: formLogin.value.username,
      password: formLogin.value.password
    })

    // 2. Jika sukses, simpan Kunci JWT ke dalam localStorage
    localStorage.setItem('jwt_token', response.data.token)
    
    // (Opsional) Simpan data role/username untuk ditampilkan di profil nanti
    localStorage.setItem('user_role', response.data.role)
    localStorage.setItem('user_name', response.data.username)

    alert('Berhasil Login!')
    
    // 3. Pindah ke halaman Dashboard
    router.push('/admin-dashboard')

  } catch (error) {
    // Menangkap error jika backend menolak (salah password / user tidak ada)
    console.error(error)
    alert('Login Gagal! Pastikan Username dan Password Anda benar.')
  }
}
</script> -->

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const formLogin = ref({
  username: '',
  password: ''
})

const prosesLogin = () => {
  // --- KODE HARDCODE SEMENTARA ---
  // Kita set username wajib "admin" dan password wajib "admin123"
  if (formLogin.value.username === 'admin' && formLogin.value.password === 'admin123') {
    
    alert('Login Berhasil (Menggunakan Hardcode)!')
    
    // Simpan token palsu ke brankas browser agar aplikasi mengira kita sudah resmi login
    localStorage.setItem('jwt_token', 'token_palsu_sementara_12345')
    localStorage.setItem('user_role', 'Admin')
    localStorage.setItem('username', 'admin')
    
    // Pindah ke halaman Dashboard
    router.push('/admin-dashboard')
    
  } else {
    alert('Login Gagal! Masukkan username: admin dan password: admin123')
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