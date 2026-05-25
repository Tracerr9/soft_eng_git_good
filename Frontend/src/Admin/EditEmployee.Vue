<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

const route = useRoute()
const router = useRouter()
const userId = route.params.id
const isLoading = ref(true)

const formKaryawan = ref({
  id: userId,
  username: '', 
  role: 'Cashier',
  password: '' 
})

const ambilDetailKaryawan = async () => {
  try {
    const response = await api.get('/api/User')
    const semuaKaryawan = response.data || []
    
    const targetUser = semuaKaryawan.find(u => (u.id || u.Id) == userId)

    if (!targetUser) {
      alert("Data karyawan tidak ditemukan.")
      router.push('/admin-employee')
      return
    }

    formKaryawan.value.username = targetUser.username || targetUser.Username
    formKaryawan.value.role = targetUser.role || targetUser.Role

  } catch (error) {
    console.error("Gagal memuat detail:", error)
  } finally {
    isLoading.value = false
  }
}

const updateData = async () => {
  try {
    isLoading.value = true
    const payload = {
      Username: formKaryawan.value.username,
      Password: formKaryawan.value.password || "", 
      Role: formKaryawan.value.role 
    }

    await api.put(`/api/User/${userId}`, payload)
    
    alert(`Sukses! Data karyawan ${formKaryawan.value.username} diperbarui dengan posisi: ${formKaryawan.value.role}.`)
    router.push('/admin-employee')
  } catch (error) {
    console.error("Gagal mengupdate:", error)
    alert("Gagal menyimpan perubahan.")
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  ambilDetailKaryawan()
})
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 class="fw-bold mb-0">Edit Karyawan</h2>
          <small class="text-muted">ID User: {{ formKaryawan.id }}</small>
        </div>
        <button class="btn btn-secondary shadow-sm" @click="router.push('/admin-employee')">⬅️ Kembali</button>
      </div>

      <div v-if="isLoading" class="text-center py-5">
        <div class="spinner-border text-primary"></div>
      </div>

      <div v-else class="card shadow-sm" style="max-width: 600px;">
        <div class="card-body">
          <form @submit.prevent="updateData">
            
            <div class="mb-3">
              <label class="form-label fw-bold">Username</label>
              <input type="text" class="form-control" v-model="formKaryawan.username" required>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Hak Akses (Role)</label>
              <select class="form-select" v-model="formKaryawan.role" 
                      :class="formKaryawan.role === 'Inactive' ? 'text-danger fw-bold border-danger' : ''">
                <option value="Cashier" class="text-dark">Cashier</option>
                <option value="Admin" class="text-dark">Admin</option>
                <option value="Inactive" class="text-danger fw-bold">Inactive (Tidak Aktif)</option>
              </select>
              <small v-if="formKaryawan.role === 'Inactive'" class="text-danger mt-1 d-block">
                Karyawan yang di-set menjadi Inactive tidak akan memiliki izin untuk login ke aplikasi.
              </small>
            </div>

            <div class="mb-4 p-3 border rounded bg-light">
              <label class="form-label fw-bold text-danger">Reset Password (Opsional)</label>
              <input type="password" class="form-control" v-model="formKaryawan.password" placeholder="Kosongkan jika tidak ingin mengubah sandi">
              <small class="text-muted">Hanya isi jika karyawan lupa password atau ingin di-reset.</small>
            </div>
            
            <button type="submit" class="btn btn-primary fw-bold w-100 py-2">Simpan Perubahan</button>
          </form>
        </div>
      </div>

    </div>
  </div>
</template>