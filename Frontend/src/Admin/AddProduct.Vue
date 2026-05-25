<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../utils/axios' // Memanggil kurir Axios
import Sidebar from '../components/Sidebar.vue'

const router = useRouter()

// Variabel disesuaikan persis dengan struktur tabel di database C# (ProductDto)
const formProduk = ref({
  sku: '',
  productName: '',
  price: 0,
  discount: 0, // Tambahan untuk diskon
  stock: 0,
  stockThreshold: 5 // Default batas menipis
})

const isLoading = ref(false)

const simpanData = async () => {
  try {
    isLoading.value = true
    
    // Paket data yang sudah disesuaikan dengan permintaan server
    const payloadData = {
      sku: formProduk.value.sku,
      productName: formProduk.value.productName,
      basePrice: formProduk.value.price, 
      discount: formProduk.value.discount, // Mengirim Diskon
      stockThreshold: formProduk.value.stockThreshold, // Mengirim Threshold
      stock: formProduk.value.stock
    }

    console.log("Mengirim data produk baru:", payloadData)

    // Perintah POST untuk memasukkan data baru ke database
    await api.post('/api/Product', payloadData)

    alert(`Sukses! Produk ${formProduk.value.productName} berhasil ditambahkan ke katalog.`)
    router.push('/product')

  } catch (error) {
    console.error("Gagal menambah produk:", error)
    
    // Menangkap pesan error dari backend jika validasi gagal atau SKU kembar
    const pesanError = error.response?.data?.title || error.response?.data?.message || 'Pastikan form terisi dengan benar dan SKU belum terpakai.'
    alert(`Gagal menambah produk!\nDetail: ${pesanError}`)
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

            <h5 class="mb-3 mt-4 border-bottom pb-2">Harga & Diskon</h5>
            <div class="row mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Harga Satuan (Rp)</label>
                <input type="number" class="form-control" v-model.number="formProduk.price" required>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold text-success">Diskon Nominal (Rp)</label>
                <input type="number" class="form-control" v-model.number="formProduk.discount">
                <small class="text-muted">Isi 0 jika tidak ada diskon awal.</small>
              </div>
            </div>

            <h5 class="mb-3 mt-4 border-bottom pb-2">Manajemen Stok</h5>
            <div class="row mb-4">
              <div class="col-md-6">
                <label class="form-label fw-bold">Stok Awal Gudang</label>
                <input type="number" class="form-control" v-model.number="formProduk.stock" required>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold text-danger">Stock Threshold (Batas Menipis)</label>
                <input type="number" class="form-control" v-model.number="formProduk.stockThreshold" required>
                <small class="text-muted">Sistem akan memberi peringatan jika stok mencapai angka ini.</small>
              </div>
            </div>

            <button type="submit" class="btn btn-success fw-bold w-100 py-2 shadow-sm" :disabled="isLoading">
              <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
              {{ isLoading ? 'Menyimpan ke Server...' : '💾 Simpan Produk Baru' }}
            </button>
          </form>
        </div>
      </div>
      
    </div>
  </div>
</template>