<script setup>
import { ref, onMounted } from 'vue'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

// State untuk menyimpan data dari backend
const riwayatTransaksi = ref([])
const isLoading = ref(true)

// Fungsi format Rupiah
const formatRupiah = (angka) => {
  return new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
    minimumFractionDigits: 0
  }).format(angka)
}

// Fungsi merapikan tanggal dari C# (contoh: 2026-05-19T10:15:00Z -> 19 Mei 2026, 10:15)
const formatTanggal = (stringTanggal) => {
  if (!stringTanggal) return '-'
  const opsi = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' }
  return new Date(stringTanggal).toLocaleDateString('id-ID', opsi)
}

// Mengambil data dari Backend
const ambilDataTransaksi = async () => {
  try {
    isLoading.value = true
    const response = await api.get('/api/Transaction') 
    
    // Pastikan kita membuat objek baru (clean object) 
    // agar tidak ada referensi balik ke data asli dari axios
    riwayatTransaksi.value = response.data.map(item => {
      return {
        id: item.transactionId,
        kasir: item.username || 'Kasir',
        tanggal: item.transactionDate,
        total: item.total,
        // Gunakan operator logika yang aman agar status tidak tertukar
        status: item.status === 'Active' ? 'Sukses' : item.status
      }
    })
  } catch (error) {
    console.error("Gagal:", error)
  } finally {
    isLoading.value = false
  }
}

// Fungsi Void (Batal) yang sudah terhubung ke backend
const voidTransaksi = async (id) => {
  const konfirmasi = confirm(`Apakah Anda yakin ingin membatalkan (Void) transaksi #${id}?`)
  if (konfirmasi) {
    try {
      // PERHATIAN: Endpoint ini ('/api/Transaction/{id}/void') adalah tebakan logika umum.
      // Jika teman backend Anda menggunakan URL lain (misal PUT /api/Transaction/cancel), ini harus disesuaikan.
      await api.post(`/api/Transaction/${id}/void`)
      
      // Jika berhasil di backend, ubah status di layar secara instan
      const tx = riwayatTransaksi.value.find(t => t.id === id)
      if (tx) {
        tx.status = 'Dibatalkan (Void)'
        alert(`Transaksi #${id} berhasil dibatalkan di sistem.`)
      }
    } catch (error) {
      console.error("Gagal void transaksi:", error)
      alert("Gagal membatalkan transaksi! Coba periksa koneksi atau tanya backend apakah endpoint Void sudah dibuat.")
    }
  }
}

// Panggil fungsi saat halaman pertama kali dibuka
onMounted(() => {
  ambilDataTransaksi()
})
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 class="fw-bold mb-0">Riwayat Transaksi</h2>
          <small class="text-muted">Daftar semua transaksi yang tercatat di sistem.</small>
        </div>
      </div>

      <div class="card shadow-sm border-0">
        <div class="card-body p-0">
          
          <div v-if="isLoading" class="text-center py-5">
            <div class="spinner-border text-primary"></div>
            <p class="mt-2 text-muted">Memuat data transaksi...</p>
          </div>

          <div v-else-if="riwayatTransaksi.length === 0" class="text-center py-5 text-muted">
            <h1 style="font-size: 3rem;"></h1>
            <h5>Belum ada transaksi.</h5>
          </div>

          <table v-else class="table table-hover align-middle mb-0">
            <thead class="table-dark">
              <tr>
                <th class="ps-3">ID Transaksi</th>
                <th>Waktu Transaksi</th>
                <th>Nama Kasir</th>
                <th>Total Belanja</th>
                <th>Status</th>
                <th>Aksi</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="tx in riwayatTransaksi" :key="tx.id" :class="{'table-danger opacity-75': tx.status.includes('Void')}">
                <td class="fw-bold ps-3 text-primary">#{{ tx.id }}</td>
                <td>{{ formatTanggal(tx.tanggal) }}</td>
                <td>{{ tx.kasir }}</td>
                <td class="fw-bold text-success">{{ formatRupiah(tx.total) }}</td>
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