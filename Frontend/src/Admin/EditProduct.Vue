<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/axios'

const route = useRoute()
const router = useRouter()

// Mengambil SKU dari URL
const skuProduk = route.params.sku || route.params.id

// Brand sudah dihapus dari daftar penampung
const formProduk = ref({
  sku: '',
  productName: '',
  price: 0,
  stock: 0,
  category: ''
})

const isLoading = ref(true)

const ambilDetailProduk = async () => {
  try {
    const response = await api.get('/api/Product')
    const semuaProduk = response.data
    const dataProdukPilihan = semuaProduk.find(
      item => item.sku === skuProduk || item.SKU === skuProduk
    )

    if (!dataProdukPilihan) {
      alert("Produk dengan SKU ini tidak ditemukan di database.")
      router.push('/product')
      return
    }
    
    formProduk.value = {
      sku: dataProdukPilihan.sku || dataProdukPilihan.SKU,
      productName: dataProdukPilihan.productName || dataProdukPilihan.ProductName || dataProdukPilihan.name,
      price: dataProdukPilihan.price || dataProdukPilihan.Price || dataProdukPilihan.basePrice || dataProdukPilihan.BasePrice || 0,
      stock: dataProdukPilihan.stock || dataProdukPilihan.Stock || 0,
      category: dataProdukPilihan.category || dataProdukPilihan.Category || 'Minuman'
    }

  } catch (error) {
    console.error("Gagal mengambil detail produk:", error)
    alert("Terjadi kesalahan saat memuat data produk.")
    router.push('/product') 
  } finally {
    isLoading.value = false
  }
}

const simpanPerubahan = async () => {
  try {
    // Paket data dikirim tanpa brand, menggunakan format camelCase
    const payloadData = {
      sku: formProduk.value.sku,
      productName: formProduk.value.productName,
      
      // TRIK JITU: Kirim 'price' dan 'basePrice' sekaligus!
      price: formProduk.value.price,
      basePrice: formProduk.value.price, 
      
      stockThreshold: 10, 
      stock: formProduk.value.stock,
      category: formProduk.value.category
    };

    console.log("Mengirim data bersih:", payloadData);

    await api.put(`/api/Product/${skuProduk}`, payloadData);

    alert('Produk berhasil diubah!');
    router.push('/product'); 

  } catch (error) {
    console.error("Gagal menyimpan produk:", error);
    alert('Gagal mengedit produk. Cek console untuk detailnya.');
  }
}

onMounted(() => {
  if (skuProduk) {
    ambilDetailProduk()
  } else {
    alert("SKU Produk tidak valid!")
    router.push('/product')
  }
})
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
          <RouterLink class="nav-link text-white bg-primary rounded" to="/product">📦 Kasir / Produk</RouterLink>
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
        <button class="btn btn-secondary shadow-sm" @click="router.push('/product')">
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

            <div class="mb-3">
              <label class="form-label fw-bold">Nama Produk</label>
              <input type="text" class="form-control" v-model="formProduk.productName" required>
            </div>

            <div class="row mb-4">
              <div class="col-md-4">
                <label class="form-label fw-bold">Harga (Rp)</label> 
                <input type="number" class="form-control" v-model.number="formProduk.price" required>
              </div>
              <div class="col-md-4">
                <label class="form-label fw-bold">Stok Gudang</label>
                <input type="number" class="form-control" v-model.number="formProduk.stock" required>
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