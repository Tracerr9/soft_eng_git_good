<script setup>
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

// Menangkap SKU dari URL browser
const productSku = route.params.sku

// Data dummy yang otomatis terisi
const formProduk = ref({
  sku: productSku,
  name: 'Susu UHT Full Cream',
  brand: 'Ultra Milk',
  price: 1.25,
  stock: 45,
  category: 'Minuman'
})

const simpanPerubahan = () => {
  alert(`Sukses! Perubahan untuk produk ${formProduk.value.name} (SKU: ${formProduk.value.sku}) berhasil disimpan.`)
  router.push('/admin-products')
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <div class="bg-dark text-white p-3" style="width: 250px;">
      <h4 class="mb-4 text-center">Smart POS</h4>
      <ul class="nav flex-column gap-2">
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
        <div>
          <h2 class="fw-bold mb-0">Edit Produk</h2>
          <small class="text-muted">Mengubah data untuk SKU: <strong>{{ formProduk.sku }}</strong></small>
        </div>
        <button class="btn btn-secondary shadow-sm" @click="router.push('/admin-products')">
          ⬅️ Kembali
        </button>
      </div>

      <div class="card shadow-sm" style="max-width: 700px;">
        <div class="card-body">
          <form @submit.prevent="simpanPerubahan">
            
            <div class="mb-3">
              <label class="form-label fw-bold">SKU Produk (Tidak Bisa Diubah)</label>
              <input type="text" class="form-control bg-light" v-model="formProduk.sku" disabled>
            </div>

            <div class="row mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Nama Produk</label>
                <input type="text" class="form-control" v-model="formProduk.name" required>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold">Merk / Brand</label>
                <input type="text" class="form-control" v-model="formProduk.brand" required>
              </div>
            </div>

            <div class="row mb-4">
              <div class="col-md-4">
                <label class="form-label fw-bold">Harga ($)</label>
                <input type="number" step="0.01" class="form-control" v-model="formProduk.price" required>
              </div>
              <div class="col-md-4">
                <label class="form-label fw-bold">Stok Gudang</label>
                <input type="number" class="form-control" v-model="formProduk.stock" required>
              </div>
              <div class="col-md-4">
                <label class="form-label fw-bold">Kategori</label>
                <select class="form-select" v-model="formProduk.category" required>
                  <option value="Minuman">Minuman</option>
                  <option value="Makanan">Makanan</option>
                  <option value="Sembako">Sembako</option>
                  <option value="Lainnya">Lainnya</option>
                </select>
              </div>
            </div>

            <button type="submit" class="btn btn-primary fw-bold w-100 py-2 shadow-sm">
              💾 Simpan Perubahan
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>