<script setup>
import { ref, computed, onMounted } from 'vue'
import Sidebar from '../components/Sidebar.vue'
import api from '../utils/axios' 

const daftarProduk = ref([])

const formatRupiah = (angka) => {
  return new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
    minimumFractionDigits: 0
  }).format(angka)
}

const ambilDataProduk = async () => {
  try {
    const response = await api.get('/api/Product') 
    console.log("Data Produk dari Backend (Kasir):", response.data)

    daftarProduk.value = response.data.map(item => ({
      sku: item.sku || item.SKU || 'N/A',
      name: item.productName || item.ProductName || item.name || 'Tanpa Nama',
      price: item.basePrice || item.BasePrice || item.price || item.Price || 0, 
      discount: item.discount || item.Discount || 0, // Ambil Diskon
      stock: item.stock || item.Stock || 0,
      stockThreshold: item.stockThreshold || item.StockThreshold || 5 
    }))

  } catch (error) {
    console.error("Gagal mengambil data produk:", error)
    alert("Gagal memuat daftar produk dari server.")
  }
}

onMounted(() => {
  ambilDataProduk()
})

const kataKunci = ref('')
const keranjang = ref([])

const produkDifilter = computed(() => {
  return daftarProduk.value.filter(produk => 
    produk.name.toLowerCase().includes(kataKunci.value.toLowerCase()) ||
    produk.sku.toLowerCase().includes(kataKunci.value.toLowerCase())
  )
})

const tambahKeKeranjang = (produk) => {
  if (produk.stock <= 0) {
    alert('Maaf, stok barang habis!')
    return
  }
  const itemAda = keranjang.value.find(item => item.sku === produk.sku)
  if (itemAda) {
    if (itemAda.jumlah >= produk.stock) {
      alert('Tidak bisa menambah! Melebihi batas stok yang ada.')
      return
    }
    itemAda.jumlah++
  } else {
    keranjang.value.push({ ...produk, jumlah: 1 })
  }
}

const hapusDariKeranjang = (sku) => {
  keranjang.value = keranjang.value.filter(item => item.sku !== sku)
}

// Total Belanja sekarang menghitung harga setelah dikurangi diskon
const totalBelanja = computed(() => {
  return keranjang.value.reduce((total, item) => {
    const hargaAkhir = item.price - item.discount
    return total + (hargaAkhir * item.jumlah)
  }, 0)
})

const prosesCheckout = async () => {
  if (keranjang.value.length === 0) return

  try {
    const detailTransaksi = keranjang.value.map(item => ({
      ProductSKU: item.sku,
      ProductAmount: item.jumlah
    }))

    const payloadTransaksi = {
      Details: detailTransaksi,
      TransactionDate: new Date().toISOString(),
      Total: totalBelanja.value
    }

    await api.post('/api/Transaction', payloadTransaksi)
    alert(`Transaksi Berhasil!`)
    
    keranjang.value = []
    ambilDataProduk()
  } catch (error) {
    console.error("Gagal memproses transaksi:", error)
    alert(`Transaksi Gagal!`)
  }
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">

    <Sidebar />
    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4 pb-3 border-bottom">
        <div>
          <h2 class="fw-bold mb-0 text-dark">Mode Kasir</h2>
          <small class="text-muted">Pilih produk untuk ditambahkan ke transaksi (Draft)</small>
        </div>
        <RouterLink to="/admin-add-product" class="btn btn-outline-primary fw-bold shadow-sm">+ Tambah Produk</RouterLink>
      </div>
      
      <div class="row">
        <div class="col-md-8 border-end pr-4">
          <div class="mb-4">
            <input type="text" class="form-control form-control-lg shadow-sm" v-model="kataKunci" placeholder="Cari nama produk atau SKU... 🔍">
          </div>
          
          <div class="row g-3">
            <div class="col-12" v-if="produkDifilter.length === 0">
              <div class="alert alert-warning text-center">Produk tidak ditemukan.</div>
            </div>

            <div class="col-md-4 col-sm-6" v-for="produk in produkDifilter" :key="produk.sku">
              <div class="card h-100 shadow-sm border-0" :class="{'opacity-50 bg-light': produk.stock <= 0}">
                <div class="card-body d-flex flex-column">
                  
                  <div class="d-flex justify-content-between align-items-start mb-2">
                    <span v-if="produk.discount > 0" class="badge bg-danger">Diskon!</span>
                    <span v-else></span>
                    
                    <span class="badge" 
                      :class="produk.stock <= produk.stockThreshold ? 'bg-danger' : (produk.stock <= (produk.stockThreshold + 5) ? 'bg-warning text-dark' : 'bg-success')">
                      Stok: {{ produk.stock }}
                    </span>
                  </div>

                  <div class="d-flex justify-content-end align-items-center mb-1">
                    <RouterLink :to="'/product/' + produk.sku" class="btn btn-sm btn-outline-secondary py-0 px-2" style="font-size: 0.75rem;">✏️ Edit</RouterLink>
                  </div>
                  
                  <h6 class="fw-bold mb-3 mt-2">{{ produk.name }}</h6>
                  
                  <div class="mt-auto d-flex justify-content-between align-items-center">
                    <div>
                      <small v-if="produk.discount > 0" class="text-muted text-decoration-line-through d-block" style="font-size: 0.75rem;">
                        {{ formatRupiah(produk.price) }}
                      </small>
                      <h5 class="text-primary fw-bold mb-0">{{ formatRupiah(produk.price - produk.discount) }}</h5>
                    </div>
                    <button class="btn btn-sm btn-primary fw-bold" @click="tambahKeKeranjang(produk)" :disabled="produk.stock <= 0">+ Add</button>
                  </div>

                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="col-md-4">
          <div class="card shadow-sm h-100 sticky-top" style="top: 20px;">
            <div class="card-header bg-white py-3">
              <h5 class="mb-0 fw-bold text-dark">📋 Detail Transaksi</h5>
            </div>
            
            <div class="card-body overflow-auto" style="max-height: 60vh;">
              <div v-if="keranjang.length === 0" class="text-center mt-5 text-muted">
                <h1 style="font-size: 3rem;">🛒</h1>
                <p>Keranjang masih kosong.</p>
              </div>

              <ul class="list-group list-group-flush" v-else>
                <li class="list-group-item px-0 py-3" v-for="item in keranjang" :key="item.sku">
                  <div class="d-flex justify-content-between align-items-start mb-1">
                    <span class="fw-bold">{{ item.name }}</span>
                    <button class="btn btn-sm text-danger p-0" @click="hapusDariKeranjang(item.sku)">✖</button>
                  </div>
                  
                  <div class="d-flex justify-content-between align-items-center text-muted small">
                    <span>{{ formatRupiah(item.price - item.discount) }} x {{ item.jumlah }}</span>
                    <span class="fw-bold text-dark">{{ formatRupiah((item.price - item.discount) * item.jumlah) }}</span>
                  </div>
                </li>
              </ul>
            </div>
            
            <div class="card-footer bg-white border-top p-3">
              <div class="d-flex justify-content-between align-items-center mb-3">
                <h5 class="text-muted mb-0">Total</h5>
                <h3 class="text-success fw-bold mb-0">{{ formatRupiah(totalBelanja) }}</h3>
              </div>
              <button class="btn btn-success btn-lg w-100 fw-bold py-3 shadow" :disabled="keranjang.length === 0" @click="prosesCheckout">💵 PROSES PEMBAYARAN</button>
            </div>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>