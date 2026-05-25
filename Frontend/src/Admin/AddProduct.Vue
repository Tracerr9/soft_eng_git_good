<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/axios' // Memanggil kurir Axios

const router = useRouter()

// Variabel disesuaikan dengan struktur tabel di database C#
const formProduk = ref({
  sku: '',
  productName: '',
  price: 0,
  stock: 0,
  category: 'Minuman'
})

const isLoading = ref(false)

const simpanData = async () => {
  try {
    isLoading.value = true
    
    // Paket data yang sudah disesuaikan dengan permintaan server (camelCase)
    const payloadData = {
      sku: formProduk.value.sku,
      productName: formProduk.value.productName,
      price: formProduk.value.price,
      basePrice: formProduk.value.price, // Trik ganda agar terbaca C#
      stockThreshold: 10, 
      stock: formProduk.value.stock,
      category: formProduk.value.category
    }

    console.log("Mengirim data produk baru:", payloadData)

    // Perintah POST untuk memasukkan data baru ke database
    await api.post('/api/Product', payloadData)

    alert(`Sukses! Produk ${formProduk.value.productName} berhasil ditambahkan ke katalog.`)
    router.push('/product')

  } catch (error) {
    console.error("Gagal menambah produk:", error)
    
    // Menangkap pesan error dari backend jika SKU sudah ada/kembar
    const pesanError = error.response?.data?.title || error.response?.data?.message || 'Pastikan SKU belum terpakai.'
    alert(`Gagal menambah produk!\nDetail: ${pesanError}`)
  } finally {
    isLoading.value = false
  }
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
          <RouterLink class="nav-link text-white bg-primary rounded" to="/product">Produk</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-transactions">Riwayat Transaksi</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-employee">Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">Logout</RouterLink>
        </li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Tambah Produk Baru</h2>
        <button class="btn btn-secondary" @click="router.push('/product')">Kembali</button>
      </div>

      <div class="card shadow-sm" style="max-width: 800px;">
        <div class="card-body">
          <form @submit.prevent="simpanData">
            
            <h5 class="mb-3 border-bottom pb-2">Informasi Dasar</h5>
            <div class="row mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">SKU (Kode Barang Unik)</label>
                <input type="text" class="form-control" v-model="formProduk.sku" required placeholder="Contoh: PRD-001">
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold">Nama Produk</label>
                <input type="text" class="form-control" v-model="formProduk.productName" required placeholder="Contoh: Susu UHT">
              </div>
            </div>

            <h5 class="mb-3 mt-4 border-bottom pb-2">Stok & Harga</h5>
            <div class="row mb-4">
              <div class="col-md-4">
                <label class="form-label fw-bold">Harga Satuan (Rp)</label>
                <input type="number" class="form-control" v-model.number="formProduk.price" required>
              </div>
              <div class="col-md-4">
                <label class="form-label fw-bold">Stok Awal Gudang</label>
                <input type="number" class="form-control" v-model.number="formProduk.stock" required>
              </div>
            </div>

            <button type="submit" class="btn btn-success fw-bold w-100 py-2" :disabled="isLoading">
              <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
              {{ isLoading ? 'Menyimpan ke Server...' : 'Simpan Produk Baru' }}
            </button>
          </form>
        </div>
      </div>
      
    </div>
  </div>
</template>