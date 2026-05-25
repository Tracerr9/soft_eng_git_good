<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

const route = useRoute()
const router = useRouter()

const skuProduk = route.params.sku || route.params.id

const formProduk = ref({
  sku: '',
  productName: '',
  price: 0,
  discount: 0, // Tambahan untuk diskon
  stock: 0,
  stockThreshold: 5
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
      discount: dataProdukPilihan.discount || dataProdukPilihan.Discount || 0, // Ambil dari server
      stock: dataProdukPilihan.stock || dataProdukPilihan.Stock || 0,
      stockThreshold: dataProdukPilihan.stockThreshold || dataProdukPilihan.StockThreshold || 5
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
    const payloadData = {
      sku: formProduk.value.sku,
      productName: formProduk.value.productName,
      price: formProduk.value.price,
      basePrice: formProduk.value.price, 
      discount: formProduk.value.discount, // Kirim Diskon
      stockThreshold: formProduk.value.stockThreshold, 
      stock: formProduk.value.stock
    };

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
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 class="fw-bold mb-0">Edit Produk</h2>
          <small class="text-muted">Mengubah data untuk SKU: <strong>{{ formProduk.sku }}</strong></small>
        </div>
        <button class="btn btn-secondary shadow-sm" @click="router.push('/product')">⬅️ Kembali</button>
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

            <div class="row mb-3">
              <div class="col-md-6">
                <label class="form-label fw-bold">Harga Awal (Rp)</label> 
                <input type="number" class="form-control" v-model.number="formProduk.price" required>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold text-success">Diskon Nominal (Rp)</label> 
                <input type="number" class="form-control" v-model.number="formProduk.discount">
                <small class="text-muted">Isi 0 jika tidak ada diskon.</small>
              </div>
            </div>

            <div class="row mb-4">
              <div class="col-md-6">
                <label class="form-label fw-bold">Stok Gudang</label>
                <input type="number" class="form-control" v-model.number="formProduk.stock" required>
              </div>
              <div class="col-md-6">
                <label class="form-label fw-bold text-danger">Stock Threshold (Batas Menipis)</label>
                <input type="number" class="form-control" v-model.number="formProduk.stockThreshold" required>
              </div>
            </div>

            <button type="submit" class="btn btn-primary fw-bold w-100 py-2 shadow-sm">Simpan Perubahan</button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>