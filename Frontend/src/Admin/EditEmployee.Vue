<script setup>
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const userId = route.params.id

// Data dummy (Nanti didapat dari GET /api/user/{userId})
const formKaryawan = ref({
  id: userId,
  username: 'Budi Santoso', 
  role: 'Cashier',
  password: '' 
})

const updateData = () => {
  // Simulasi PUT /api/user/{userId}
  alert(`Sukses! Data karyawan ${formKaryawan.value.username} berhasil diperbarui.`)
  router.push('/admin-employee')
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <div class="bg-dark text-white p-3" style="width: 250px;">
      <h4 class="mb-4 text-center">Smart POS</h4>
      <ul class="nav flex-column gap-2">
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-notifications">🔔 Notifikasi System</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-dashboard">📊 Dashboard</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/product">📦 Kasir / Produk</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-transactions">🧾 Riwayat Transaksi</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-employee">👥 Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">🚪 Logout</RouterLink>
        </li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 class="fw-bold mb-0">Edit Karyawan</h2>
          <small class="text-muted">ID User: {{ formKaryawan.id }}</small>
        </div>
        <button class="btn btn-secondary shadow-sm" @click="router.push('/admin-employee')">⬅️ Kembali</button>
      </div>

      <div class="card shadow-sm" style="max-width: 600px;">
        <div class="card-body">
          <form @submit.prevent="updateData">
            
            <div class="mb-3">
              <label class="form-label fw-bold">Username</label>
              <input type="text" class="form-control" v-model="formKaryawan.username" required>
            </div>

            <div class="mb-3">
              <label class="form-label fw-bold">Hak Akses (Role)</label>
              <select class="form-select" v-model="formKaryawan.role" required>
                <option value="Cashier">Cashier</option>
                <option value="Admin">Admin</option>
              </select>
            </div>

            <div class="mb-4 p-3 border rounded bg-light">
              <label class="form-label fw-bold text-danger">Reset Password (Opsional)</label>
              <input type="password" class="form-control" v-model="formKaryawan.password" placeholder="Kosongkan jika tidak ingin mengubah sandi">
              <small class="text-muted">Hanya isi jika karyawan lupa password atau ingin di-reset.</small>
            </div>
            
            <button type="submit" class="btn btn-primary fw-bold w-100 py-2">💾 Simpan Perubahan</button>
          </form>
        </div>
      </div>

    </div>
  </div>
</template>