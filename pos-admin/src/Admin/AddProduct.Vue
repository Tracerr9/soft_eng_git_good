<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

// Variabel penampung data yang diketik di form
const formProduk = ref({
  nama: '',
  merk: '',
  hargaSatuan: 0,
  hargaBox: 0,
  isiBox: 0
})

const simpanData = () => {
  // Simulasi menyimpan ke database
  alert(`Sukses! Produk ${formProduk.value.nama} berhasil ditambahkan ke katalog.`)
  
  // Setelah sukses, otomatis pindah kembali ke halaman daftar produk
  router.push('/admin-products')
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
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-products">📦 Kasir / Produk</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-transactions">🧾 Riwayat Transaksi</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-employee">👥 Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">🚪 Logout</RouterLink>
        </li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Tambah Produk Baru</h2>
        <button class="btn btn-secondary" @click="router.push('/admin-products')">Kembali</button>
      </div>

      <div class="card shadow-sm" style="max-width: 800px;">
        <div class="card-body">
          <form @submit.prevent="simpanData">
            
            <h5 class="mb-3 border-bottom pb-2">Informasi Dasar</h5>
            <div class="row mb-3">
              <div class="col-md-6">
                <label class="form-label">Nama Produk</label>
                <input type="text" class="form-control" v-model="formProduk.nama" required placeholder="Contoh: Susu UHT">
              </div>
              <div class="col-md-6">
                <label class="form-label">Merk</label>
                <input type="text" class="form-control" v-model="formProduk.merk" required placeholder="Contoh: Indomilk">
              </div>
            </div>

            <h5 class="mb-3 mt-4 border-bottom pb-2">Informasi Harga & Box</h5>
            <div class="row mb-4">
              <div class="col-md-4">
                <label class="form-label">Harga Satuan ($)</label>
                <input type="number" step="0.01" class="form-control" v-model="formProduk.hargaSatuan" required>
              </div>
              <div class="col-md-4">
                <label class="form-label">Harga Per Box ($)</label>
                <input type="number" step="0.01" class="form-control" v-model="formProduk.hargaBox" required>
              </div>
              <div class="col-md-4">
                <label class="form-label">Isi Per Box (Pcs)</label>
                <input type="number" class="form-control" v-model="formProduk.isiBox" required>
              </div>
            </div>

            <button type="submit" class="btn btn-success fw-bold w-100 py-2">💾 Simpan Produk Baru</button>
          </form>
        </div>
      </div>
      
    </div>
  </div>
</template>