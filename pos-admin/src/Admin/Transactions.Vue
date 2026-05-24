<script setup>
import { ref } from 'vue'

// Data Dummy Transaksi (Nanti diganti dengan GET /api/transaction)
const riwayatTransaksi = ref([
  { id: 1001, kasir: 'Budi Santoso', tanggal: '2026-05-19 10:15:00', total: 15.50, status: 'Sukses' },
  { id: 1002, kasir: 'Siti Aminah', tanggal: '2026-05-19 11:30:22', total: 8.75, status: 'Sukses' },
  { id: 1003, kasir: 'Andi Wijaya', tanggal: '2026-05-19 12:05:10', total: 45.00, status: 'Dibatalkan (Void)' },
  { id: 1004, kasir: 'Budi Santoso', tanggal: '2026-05-19 14:20:00', total: 3.20, status: 'Sukses' }
])

const voidTransaksi = (id) => {
  const konfirmasi = confirm(`Apakah Anda yakin ingin membatalkan (Void) transaksi #${id}?`)
  if (konfirmasi) {
    // Simulasi Void (Nanti memanggil POST /api/transaction/{id}/void)
    const tx = riwayatTransaksi.value.find(t => t.id === id)
    if (tx) {
      tx.status = 'Dibatalkan (Void)'
      alert(`Transaksi #${id} berhasil dibatalkan.`)
    }
  }
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
          <RouterLink class="nav-link text-white" to="/admin-products">📦 Kasir / Produk</RouterLink>
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
        <h2>Riwayat Transaksi</h2>
        <button class="btn btn-outline-secondary">⬇️ Export Laporan</button>
      </div>

      <div class="card shadow-sm">
        <div class="card-body">
          <table class="table table-hover align-middle">
            <thead class="table-dark">
              <tr>
                <th>ID Transaksi</th>
                <th>Waktu Transaksi</th>
                <th>Nama Kasir</th>
                <th>Total Belanja</th>
                <th>Status</th>
                <th>Aksi</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="tx in riwayatTransaksi" :key="tx.id" :class="{'table-danger opacity-75': tx.status.includes('Void')}">
                <td class="fw-bold">#{{ tx.id }}</td>
                <td>{{ tx.tanggal }}</td>
                <td>{{ tx.kasir }}</td>
                <td class="fw-bold">${{ tx.total.toFixed(2) }}</td>
                <td>
                  <span :class="{'badge bg-success': tx.status === 'Sukses', 'badge bg-danger': tx.status.includes('Void')}">
                    {{ tx.status }}
                  </span>
                </td>
                <td>
                  <RouterLink 
                    :to="'/admin-transaction/' + tx.id" 
                    class="btn btn-sm btn-info text-white me-2 fw-bold"
                    >
                    Detail
                </RouterLink>
                <button 
                    v-if="tx.status === 'Sukses'" 
                    class="btn btn-sm btn-danger fw-bold"
                    @click="voidTransaksi(tx.id)"
                  >
                    Void
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      
    </div>
  </div>
</template>