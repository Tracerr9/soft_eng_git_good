<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const formKaryawan = ref({
  username: '',
  password: '',
  role: 'Cashier' // Default role
})

const simpanData = () => {
  // Simulasi POST /api/user
  alert(`Sukses! Karyawan ${formKaryawan.value.username} berhasil didaftarkan sebagai ${formKaryawan.value.role}.`)
  router.push('/admin-employee')
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <div class="bg-dark text-white p-3" style="width: 250px;">
      <h4 class="mb-4 text-center">Smart POS</h4>
      <ul class="nav flex-column gap-2">
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-notifications">Notifikasi System</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-dashboard">Dashboard</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/product">Produk</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-transactions">Riwayat Transaksi</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-employee">Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">Logout</RouterLink>
        </li>
      </ul>
    </div>

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

            <button type="submit" class="btn btn-success fw-bold w-100 py-2">Simpan Karyawan</button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>