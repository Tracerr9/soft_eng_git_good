<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../utils/axios'

const route = useRoute()
const notifikasiProduk = ref([])

const ambilNotifikasi = async () => {
  try {
    const response = await api.get('/api/Notification/Products')
    if (response.status === 204 || !response.data) {
      notifikasiProduk.value = []
      return
    }
    notifikasiProduk.value = response.data
  } catch (error) {
    console.error("Gagal mengambil notifikasi sidebar:", error)
  }
}

onMounted(() => {
  ambilNotifikasi()
})

const totalNotifikasi = computed(() => notifikasiProduk.value.length)

// Fungsi untuk memberi warna biru (aktif) pada menu yang sedang dibuka
const isActive = (path) => route.path === path || route.path.startsWith(path + '/')
</script>

<template>
  <div class="bg-dark text-white p-3 flex-shrink-0" style="width: 250px;">
    <h4 class="mb-4 text-center">Smart POS</h4>
    <ul class="nav flex-column gap-2">
      <li class="nav-item">
        <RouterLink class="nav-link text-white d-flex justify-content-between align-items-center" 
          :class="{'bg-primary rounded': isActive('/admin-notifications')}" to="/admin-notifications">
          <span>Notifikasi System</span>
          <span v-if="totalNotifikasi > 0" 
              class="badge bg-danger shadow-sm d-flex justify-content-center align-items-center" 
              style="font-size: 0.8rem; width: 24px; height: 24px; padding: 0; border-radius: 50%;">
          {{ totalNotifikasi }}
        </span>
        </RouterLink>
      </li>
      <li class="nav-item">
        <RouterLink class="nav-link text-white" :class="{'bg-primary rounded': isActive('/admin-dashboard')}" to="/admin-dashboard">Dashboard</RouterLink>
      </li>
      <li class="nav-item">
        <RouterLink class="nav-link text-white" :class="{'bg-primary rounded': isActive('/product')}" to="/product">Produk</RouterLink>
      </li>
      <li class="nav-item">
        <RouterLink class="nav-link text-white" :class="{'bg-primary rounded': isActive('/admin-transactions')}" to="/admin-transactions">Riwayat Transaksi</RouterLink>
      </li>
      <li class="nav-item">
        <RouterLink class="nav-link text-white" :class="{'bg-primary rounded': isActive('/admin-employee')}" to="/admin-employee">Kelola Karyawan</RouterLink>
      </li>
      <li class="nav-item mt-5">
        <RouterLink class="nav-link text-danger" to="/">Logout</RouterLink>
      </li>
    </ul>
  </div>
</template>