<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Sidebar from '../components/Sidebar.vue'
import api from '../utils/axios'

const router = useRouter()

const notifikasiProduk = ref([])
const isLoading = ref(true)

const ambilNotifikasi = async () => {
  try {
    isLoading.value = true
    const response = await api.get('/api/Notification/Products')

    // Jika server merespon 204 (No Content), berarti tidak ada notifikasi
    if (response.status === 204 || !response.data) {
      notifikasiProduk.value = []
      return
    }

    // Mapping data dari backend
    notifikasiProduk.value = response.data.map(item => {
      const stock = item.stock || item.Stock || 0
      const threshold = item.stockThreshold || item.StockThreshold || 0
      
      // Logika level peringatan: 
      // Jika stok sangat kritis (misal <= separuh dari threshold) atau 0
      let level = 'warning'
      let message = 'Stok menipis di bawah batas aman.'
      
      if (stock === 0 || stock <= (threshold / 2)) {
        level = 'danger'
        message = 'Stok kritis! Segera lakukan pemesanan ulang.'
      }

      return {
        sku: item.sku || item.SKU,
        name: item.productName || item.ProductName,
        stock: stock,
        limit: threshold,
        message: message,
        level: level
      }
    })

  } catch (error) {
    console.error("Gagal mengambil data notifikasi:", error)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  ambilNotifikasi()
})

// Menghitung jumlah notifikasi yang butuh tindakan segera (level danger)
const jumlahKritis = computed(() => {
  return notifikasiProduk.value.filter(n => n.level === 'danger').length
})

// Menghitung total SEMUA notifikasi (untuk ditampilkan di gelembung merah sidebar)
const totalNotifikasi = computed(() => {
  return notifikasiProduk.value.length
})
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom">
        <div>
          <h2 class="fw-bold mb-0 text-dark">Pusat Notifikasi</h2>
          <small class="text-muted">Pemberitahuan otomatis mengenai kondisi inventaris gudang</small>
        </div>
        <span v-if="jumlahKritis > 0" class="badge bg-danger p-2 fs-6 shadow-sm">
          ⚠️ {{ jumlahKritis }} Produk Butuh Restock Instan
        </span>
      </div>

      <div v-if="isLoading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status"></div>
        <p class="mt-2 text-muted">Memeriksa stok gudang...</p>
      </div>

      <div v-else class="row g-3">
        <div class="col-12" v-if="notifikasiProduk.length === 0">
          <div class="card p-5 text-center shadow-sm border-0">
            <h1 class="text-success mb-2" style="font-size: 4rem;">✅</h1>
            <h4 class="fw-bold">Semua Aman!</h4>
            <p class="text-muted mb-0">Stok seluruh produk di gudang masih berada di atas batas minimum (Threshold).</p>
          </div>
        </div>

        <div class="col-12" v-for="notif in notifikasiProduk" :key="notif.sku">
          <div class="card shadow-sm border-start border-4" :class="notif.level === 'danger' ? 'border-danger' : 'border-warning'">
            <div class="card-body d-flex justify-content-between align-items-center p-3">
              <div>
                <div class="d-flex align-items-center gap-2 mb-1">
                  <span class="badge fw-bold" :class="notif.level === 'danger' ? 'bg-danger' : 'bg-warning text-dark'">
                    {{ notif.level === 'danger' ? 'KRITIS' : 'PERINGATAN' }}
                  </span>
                  <strong class="text-dark fs-5">{{ notif.name }}</strong>
                  <span class="text-muted small">| SKU: {{ notif.sku }}</span>
                </div>
                <p class="mb-0 text-secondary">{{ notif.message }}</p>
              </div>

              <div class="text-end d-flex align-items-center gap-4">
                <div class="text-end">
                  <span class="text-muted small d-block">Stok Saat Ini</span>
                  <span class="fw-bold fs-4" :class="notif.level === 'danger' ? 'text-danger' : 'text-warning-custom'">
                    {{ notif.stock }}
                  </span>
                  <span class="text-muted small"> / Batas: {{ notif.limit }}</span>
                </div>
                
                <RouterLink :to="'/product/' + notif.sku" class="btn btn-sm btn-outline-primary fw-bold px-4 py-2">
                  Restock
                </RouterLink>
              </div>
            </div>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<style scoped>
.text-warning-custom {
  color: #d97706;
}
.border-danger {
  border-left-color: #dc3545 !important;
}
.border-warning {
  border-left-color: #ffc107 !important;
}
</style>