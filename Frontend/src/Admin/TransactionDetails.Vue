<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/axios'
import Sidebar from '../components/Sidebar.vue'

const route = useRoute()
const router = useRouter()
const transactionId = route.params.id

const detailTransaksi = ref(null)
const isLoading = ref(true)

const formatRupiah = (angka) => {
  return new Intl.NumberFormat('id-ID', { style: 'currency', currency: 'IDR', minimumFractionDigits: 0 }).format(angka)
}

const formatTanggal = (tgl) => {
  return new Date(tgl).toLocaleString('id-ID', { dateStyle: 'long', timeStyle: 'short' })
}

const ambilDetail = async () => {
  try {
    isLoading.value = true
    const response = await api.get(`/api/Transaction/${transactionId}`)
    const data = response.data
    
    // DEBUG: Print data agar kita tahu apa isinya
    console.log("DATA DARI SERVER:", data) 

    detailTransaksi.value = {
      id: data.transactionId || data.id,
      kasir: data.username || 'Admin',
      tanggal: data.transactionDate,
      status: data.status,
      total: data.total,
      // Kita coba cari 'details' atau 'items'. Jika tidak ada, kita buat array kosong.
      items: (data.details || data.items || []).map(item => ({
        sku: item.productSKU || item.sku || 'N/A',
        name: item.productName || item.name || 'Produk',
        price: item.price || 0,
        qty: item.productAmount || item.qty || 0,
        subtotal: (item.price || 0) * (item.productAmount || item.qty || 0)
      }))
    }
  } catch (error) {
    console.error("Gagal memuat detail:", error)
  } finally {
    isLoading.value = false
  }
}

onMounted(ambilDetail)

</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <Sidebar />

    <div class="p-4 flex-grow-1 bg-light">
      
      <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom">
        <div>
          <h2 class="fw-bold mb-0 text-dark">Detail Transaksi</h2>
          <small class="text-muted">ID Transaksi: #{{ transactionId }}</small>
        </div>
        <button class="btn btn-secondary shadow-sm fw-bold" @click="router.push('/admin-transactions')">
          ⬅️ Kembali ke Riwayat
        </button>
      </div>

      <div v-if="isLoading" class="text-center py-5">
        <div class="spinner-border text-primary"></div>
        <p class="mt-2 text-muted">Memuat data...</p>
      </div>

      <div v-else-if="detailTransaksi" class="row">
        <div class="col-md-4 mb-4">
          <div class="card shadow-sm h-100 border-0 border-top border-primary border-4">
            <div class="card-header bg-white fw-bold border-0 pt-3">📋 Informasi Umum</div>
            <div class="card-body">
              <p class="mb-1 text-muted small">Waktu Transaksi</p>
              <h6 class="mb-3 fw-bold">{{ formatTanggal(detailTransaksi.tanggal) }}</h6>
              
              <p class="mb-1 text-muted small">Nama Kasir</p>
              <h6 class="mb-3 fw-bold text-primary">{{ detailTransaksi.kasir }}</h6>
              
              <p class="mb-1 text-muted small">Status</p>
              <span class="badge px-3 py-2" :class="detailTransaksi.status === 'Sukses' ? 'bg-success' : 'bg-danger'">
                {{ detailTransaksi.status }}
              </span>
            </div>
          </div>
        </div>

        <div class="col-md-8 mb-4">
          <div class="card shadow-sm h-100 border-0">
            <div class="card-header bg-white fw-bold border-bottom pt-3">🛒 Daftar Item (Struk)</div>
            <div class="card-body p-0">
              <table class="table table-hover mb-0">
                <thead class="table-light">
                  <tr>
                    <th class="ps-4">SKU</th>
                    <th>Nama Produk</th>
                    <th>Harga</th>
                    <th class="text-center">Qty</th>
                    <th class="text-end pe-4">Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in detailTransaksi.items" :key="item.sku">
                    <td class="ps-4 text-muted small">{{ item.sku }}</td>
                    <td class="fw-bold">{{ item.name }}</td>
                    <td>{{ formatRupiah(item.price) }}</td>
                    <td class="text-center">
                      <span class="badge bg-secondary rounded-pill">{{ item.qty }}</span>
                    </td>
                    <td class="text-end pe-4 fw-bold">{{ formatRupiah(item.subtotal) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="card-footer bg-white text-end py-3 pe-4 border-top-0">
              <span class="text-muted me-3">Total Pembayaran:</span>
              <h3 class="fw-bold text-success d-inline-block mb-0">{{ formatRupiah(detailTransaksi.total) }}</h3>
            </div>
          </div>
        </div>
      </div>
      
    </div>
  </div>
</template>