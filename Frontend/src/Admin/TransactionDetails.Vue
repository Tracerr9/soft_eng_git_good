<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

// Mengambil ID dari URL (contoh: /admin-transaction/1001)
const transactionId = route.params.id

// Data Dummy Detail Transaksi (Nanti diganti dengan GET /api/transaction/{transactionId})
const detailTransaksi = ref({
  id: transactionId,
  kasir: 'Budi Santoso',
  tanggal: '2026-05-19 10:15:00',
  status: 'Sukses',
  subtotal: 14.50,
  pajak: 1.00,
  total: 15.50,
  items: [
    { sku: 'PRD-001', name: 'Susu UHT Full Cream', price: 1.25, qty: 2, subtotal: 2.50 },
    { sku: 'PRD-003', name: 'Beras Premium 5Kg', price: 8.75, qty: 1, subtotal: 8.75 },
    { sku: 'PRD-004', name: 'Minyak Goreng 2L', price: 3.25, qty: 1, subtotal: 3.25 }
  ]
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
          <RouterLink class="nav-link text-white" to="/products">📦 Kasir / Produk</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-transactions">🧾 Riwayat Transaksi</RouterLink>
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
          <h2 class="fw-bold mb-0">Detail Transaksi #{{ detailTransaksi.id }}</h2>
          <small class="text-muted">Rincian struk pembelian</small>
        </div>
        <button class="btn btn-secondary shadow-sm" @click="router.push('/admin-transactions')">
          ⬅️ Kembali
        </button>
      </div>

      <div class="row">
        <div class="col-md-4 mb-4">
          <div class="card shadow-sm h-100">
            <div class="card-header bg-white fw-bold">Informasi Umum</div>
            <div class="card-body">
              <p class="mb-1 text-muted small">Waktu Transaksi</p>
              <h6 class="mb-3">{{ detailTransaksi.tanggal }}</h6>
              
              <p class="mb-1 text-muted small">Nama Kasir</p>
              <h6 class="mb-3">{{ detailTransaksi.kasir }}</h6>
              
              <p class="mb-1 text-muted small">Status</p>
              <span :class="{'badge bg-success': detailTransaksi.status === 'Sukses', 'badge bg-danger': detailTransaksi.status.includes('Void')}">
                {{ detailTransaksi.status }}
              </span>
            </div>
          </div>
        </div>

        <div class="col-md-8 mb-4">
          <div class="card shadow-sm h-100">
            <div class="card-header bg-white fw-bold">Daftar Item (Struk)</div>
            <div class="card-body p-0">
              <table class="table table-borderless mb-0">
                <thead class="table-light border-bottom">
                  <tr>
                    <th class="ps-3">SKU</th>
                    <th>Nama Produk</th>
                    <th>Harga</th>
                    <th class="text-center">Qty</th>
                    <th class="text-end pe-3">Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in detailTransaksi.items" :key="item.sku" class="border-bottom">
                    <td class="ps-3 text-muted small">{{ item.sku }}</td>
                    <td class="fw-bold">{{ item.name }}</td>
                    <td>${{ item.price.toFixed(2) }}</td>
                    <td class="text-center">{{ item.qty }}</td>
                    <td class="text-end pe-3 fw-bold">${{ item.subtotal.toFixed(2) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
            
            <div class="card-footer bg-white text-end py-3 pe-4">
              <div class="text-muted mb-1">Subtotal: ${{ detailTransaksi.subtotal.toFixed(2) }}</div>
              <div class="text-muted mb-2">Pajak (Tax): ${{ detailTransaksi.pajak.toFixed(2) }}</div>
              <h4 class="fw-bold text-success mb-0">Total: ${{ detailTransaksi.total.toFixed(2) }}</h4>
            </div>
          </div>
        </div>
      </div>
      
    </div>
  </div>
</template>