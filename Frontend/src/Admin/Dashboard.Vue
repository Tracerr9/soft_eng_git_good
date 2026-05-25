<script setup>
import { ref, onMounted } from 'vue'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

const jumlahProduk = ref(0)
const jumlahKaryawan = ref(0)
const transaksiHariIni = ref(0)
const produkStokMenipis = ref([])

const loadDashboardData = async () => {
  try {
    // 1. Ambil data produk untuk hitung total
    const resProduk = await api.get('/api/Product')
    jumlahProduk.value = resProduk.data ? resProduk.data.length : 0
    
    // 2. Mengambil Peringatan Stok langsung dari Endpoint Khusus Notifikasi C#
    const resNotif = await api.get('/api/Notification/Products')
    // Jika server merespon 204 (No Content), artinya stok aman semua
    produkStokMenipis.value = resNotif.data ? resNotif.data.map(item => ({
      sku: item.sku || item.SKU,
      productName: item.productName || item.ProductName,
      stock: item.stock || item.Stock
    })) : []

    // 3. Ambil data transaksi untuk hitung transaksi hari ini
    const resTransaksi = await api.get('/api/Transaction')
    const hariIni = new Date().toISOString().split('T')[0]
    if (resTransaksi.data) {
      transaksiHariIni.value = resTransaksi.data.filter(t => 
        (t.transactionDate || t.TransactionDate).startsWith(hariIni)
      ).length
    }

    // 4. Ambil data karyawan
    const resKaryawan = await api.get('/api/User') // Sesuai endpoint UserController Anda
    jumlahKaryawan.value = resKaryawan.data ? resKaryawan.data.length : 0
  } catch (error) {
    console.error("Gagal memuat data dashboard:", error)
  }
}

onMounted(() => {
  loadDashboardData()
})
</script>

<template>
  
  <div class="d-flex" style="min-height: 100vh;">
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <h2 class="mb-4">Dashboard Admin</h2>

      <div class="row mb-4">
        <div class="col-md-4">
          <div class="card text-white bg-info mb-3 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">Total Produk</h5>
              <h2 class="card-text">{{ jumlahProduk }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card text-white bg-success mb-3 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">Transaksi Hari Ini</h5>
              <h2 class="card-text">{{ transaksiHariIni }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card text-white bg-warning mb-3 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">Total Karyawan</h5>
              <h2 class="card-text">{{ jumlahKaryawan }}</h2>
            </div>
          </div>
        </div>
      </div>

      <div class="card shadow-sm">
        <div class="card-header bg-white">
          <h5 class="mb-0 text-danger fw-bold">⚠️ Peringatan Stok Menipis</h5>
        </div>
        <div class="card-body">
          <table class="table table-hover align-middle">
            <thead class="table-light">
              <tr>
                <th>ID</th>
                <th>Nama Barang</th>
                <th>Sisa Stok</th>
                <th>Aksi</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="produk in produkStokMenipis" :key="produk.sku">
                <td>{{ produk.sku }}</td>
                <td class="fw-bold">{{ produk.productName }}</td>
                <td class="text-danger fw-bold">{{ produk.stock }}</td>
                <td><RouterLink class="btn btn-sm btn-primary" to="/admin-notifications">Restock</RouterLink></td>
              </tr>
              <tr v-if="produkStokMenipis.length === 0">
                <td colspan="4" class="text-center text-muted">Stok aman!</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>