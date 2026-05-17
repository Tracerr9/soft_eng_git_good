<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

const prosesLogin = () => {
  // Simulasi login sukses, langsung pindah ke dashboard
  router.push('/admin-dashboard')
}
</script>

<template>
  <div class="d-flex justify-content-center align-items-center bg-light" style="min-height: 100vh;">
    <div class="card shadow-sm p-4" style="width: 400px;">
      <h3 class="text-center mb-4 fw-bold">Smart POS Login</h3>
      
      <form @submit.prevent="prosesLogin">
        <div class="mb-3">
          <label class="form-label">Username / Email</label>
          <input type="text" class="form-control" placeholder="Masukkan username">
        </div>
        
        <div class="mb-4">
          <label class="form-label">Password</label>
          <input type="password" class="form-control" placeholder="Masukkan password">
        </div>
        
        <button type="submit" class="btn btn-primary w-100 fw-bold">Login</button>
      </form>
      
    </div>
  </div>
</template>