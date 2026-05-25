<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

// Data Dummy Peringatan Produk (Nanti diganti dengan GET /api/notification/product)
const notifikasiProduk = ref([
  { sku: 'PRD-004', name: 'Minyak Goreng 2L', brand: 'Bimoli', stock: 2, limit: 5, message: 'Stok kritis! Segera lakukan pemesanan ulang.', level: 'danger' },
  { sku: 'PRD-003', name: 'Beras Premium 5Kg', brand: 'Sania', stock: 8, limit: 10, message: 'Stok menipis di bawah batas aman.', level: 'warning' },
  { sku: 'PRD-002', name: 'Kopi Bubuk', brand: 'Kapal Api', stock: 12, limit: 15, message: 'Stok mendekati batas minimum.', level: 'warning' }
])

// Menghitung jumlah notifikasi yang butuh tindakan segera (level danger)
const jumlahKritis = computed(() => {
  return notifikasiProduk.value.filter(n => n.level === 'danger').length
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
          <RouterLink class="nav-link text-white" to="/product">📦 Kasir / Produk</RouterLink>
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
      <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom">
        <div>
          <h2 class="fw-bold mb-0 text-dark">Pusat Notifikasi</h2>
          <small class="text-muted">Pemberitahuan otomatis mengenai kondisi inventaris gudang</small>
        </div>
        <span v-if="jumlahKritis > 0" class="badge bg-danger p-2 fs-6">
          ⚠️ {{ jumlahKritis }} Produk Butuh Restock Instan
        </span>
      </div>

      <div class="row g-3">
        <div class="col-12" v-if="notifikasiProduk.length === 0">
          <div class="card p-5 text-center shadow-sm">
            <h1 class="text-success mb-2">✅</h1>
            <h5>Semua Aman!</h5>
            <p class="text-muted mb-0">Stok seluruh produk di gudang masih berada di atas batas minimum.</p>
          </div>
        </div>

        <div class="col-12" v-for="notif in notifikasiProduk" :key="notif.sku">
          <div class="card shadow-sm border-start border-4" :class="notif.level === 'danger' ? 'border-danger' : 'border-warning'">
            <div class="card-body d-flex justify-content-between align-items-center p-3">
              <div>
                <div class="d-flex align-items-center gap-2 mb-1">
                  <span class="badge" :class="notif.level === 'danger' ? 'bg-danger' : 'bg-warning text-dark'">
                    {{ notif.level === 'danger' ? 'KRITIS' : 'PERINGATAN' }}
                  </span>
                  <strong class="text-dark">{{ notif.name }} ({{ notif.brand }})</strong>
                  <span class="text-muted small">| SKU: {{ notif.sku }}</span>
                </div>
                <p class="mb-0 text-secondary small">{{ notif.message }}</p>
              </div>

              <div class="text-end d-flex align-items-center gap-4">
                <div>
                  <span class="text-muted small d-block">Stok Saat Ini</span>
                  <span class="fw-bold fs-5" :class="notif.level === 'danger' ? 'text-danger' : 'text-warning-custom'">
                    {{ notif.stock }} Pcs
                  </span>
                  <span class="text-muted small"> / Batas: {{ notif.limit }}</span>
                </div>
                
                <RouterLink :to="'/admin-edit-product/' + notif.sku" class="btn btn-sm btn-outline-primary fw-bold px-3">
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