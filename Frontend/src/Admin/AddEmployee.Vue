<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

const router = useRouter()
const isLoading = ref(false)

const formKaryawan = ref({
  username: '',
  password: '',
  role: 'Cashier' // Default role
})

const simpanData = async () => {
  try {
    isLoading.value = true
    const payload = {
      Username: formKaryawan.value.username,
      Password: formKaryawan.value.password,
      Role: formKaryawan.value.role
    }

    await api.post('/api/User', payload)
    
    alert(`Sukses! Karyawan ${formKaryawan.value.username} berhasil didaftarkan.`)
    router.push('/admin-employee')
  } catch (error) {
    console.error("Gagal menambah karyawan:", error)
    const pesan = error.response?.data?.message || 'Gagal menyimpan data.'
    alert(`Error: ${pesan}`)
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Tambah Karyawan Baru</h2>
        <button class="btn btn-secondary shadow-sm" @click="router.push('/admin-employee')">⬅️ Kembali</button>
      </div>

      <div class="card shadow-sm" style="max-width: 600px;">
        <div class="card-body">
          <form @submit.prevent="simpanData">
            
            <div class="mb-3">
              <label class="form-label fw-bold">Username</label>
              <input type="text" class="form-control" v-model="formKaryawan.username" required placeholder="Masukkan nama pengguna">
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Password</label>
              <input type="password" class="form-control" v-model="formKaryawan.password" required placeholder="Buat kata sandi sementara">
            </div>

            <div class="mb-4">
              <label class="form-label fw-bold">Hak Akses (Role)</label>
              <select class="form-select" v-model="formKaryawan.role" required>
                <option value="Cashier">Cashier</option>
                <option value="Admin">Admin</option>
                
              </select>
            </div>

            <button type="submit" class="btn btn-success fw-bold w-100 py-2" :disabled="isLoading">
              {{ isLoading ? 'Menyimpan...' : 'Simpan Karyawan' }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>