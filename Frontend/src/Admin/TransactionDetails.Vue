<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../utils/axios'

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
    <div class="bg-dark text-white p-3" style="width: 250px;">
      <h4 class="mb-4 text-center">Smart POS</h4>
      <ul class="nav flex-column gap-2">
        <li class="nav-item"><RouterLink class="nav-link text-white" to="/admin-transactions">⬅️ Kembali ke Riwayat</RouterLink></li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 bg-light">
      <div v-if="isLoading" class="text-center py-5">Memuat data...</div>

      <div v-else-if="detailTransaksi" class="row">
        <div class="col-md-4 mb-4">
          <div class="card shadow-sm h-100">
            <div class="card-header bg-white fw-bold">Informasi Umum</div>
            <div class="card-body">
              <p class="mb-1 text-muted small">Waktu Transaksi</p>
              <h6 class="mb-3">{{ formatTanggal(detailTransaksi.tanggal) }}</h6>
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
                    <td>{{ formatRupiah(item.price) }}</td>
                    <td class="text-center">{{ item.qty }}</td>
                    <td class="text-end pe-3 fw-bold">{{ formatRupiah(item.subtotal) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="card-footer bg-white text-end py-3 pe-4">
              <h4 class="fw-bold text-success mb-0">Total: {{ formatRupiah(detailTransaksi.total) }}</h4>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>