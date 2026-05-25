<script setup>
import { ref, onMounted } from 'vue'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

const daftarKaryawan = ref([])
const isLoading = ref(true)

const ambilDataKaryawan = async () => {
  try {
    isLoading.value = true
    const response = await api.get('/api/User')
    
    // Mapping data agar sesuai dan aman
    daftarKaryawan.value = response.data ? response.data.map(emp => ({
      id: emp.id || emp.Id,
      nama: emp.username || emp.Username,
      role: emp.role || emp.Role
    })) : []

  } catch (error) {
    console.error("Gagal memuat karyawan:", error)
    alert("Gagal memuat data karyawan dari server.")
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  ambilDataKaryawan()
})

const hapusKaryawan = () => {
  alert("Maaf, API/Endpoint untuk menghapus karyawan belum dibuat di server C# Anda.")
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">

    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Kelola Karyawan</h2>
        <RouterLink to="/admin-add-employee" class="btn btn-success fw-bold">+ Tambah Karyawan</RouterLink>
      </div>

      <div class="card shadow-sm border-0">
        <div class="card-body p-0">
          
          <div v-if="isLoading" class="text-center py-5">
            <div class="spinner-border text-primary"></div>
          </div>

          <table v-else class="table table-hover align-middle mb-0">
            <thead class="table-dark">
              <tr>
                <th class="ps-3">ID</th>
                <th>Username</th>
                <th>Posisi (Role)</th>
                <th>Aksi</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="emp in daftarKaryawan" :key="emp.id">
                <td class="fw-bold ps-3">{{ emp.id }}</td>
                <td class="fw-bold text-primary">{{ emp.nama }}</td>
                <td>
                  <span class="badge" :class="emp.role === 'Cashier' ? 'bg-success' : 'bg-danger'">
                    {{ emp.role }}
                  </span>
                </td>
                <td>
                  <RouterLink :to="'/admin-edit-employee/' + emp.id" class="btn btn-sm btn-outline-primary me-2 fw-bold">
                    Edit
                  </RouterLink>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>