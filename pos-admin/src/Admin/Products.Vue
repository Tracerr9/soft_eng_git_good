<script setup>
import { ref, computed } from 'vue'

// Data Dummy (Nanti akan diganti dengan GET /api/product)
const daftarProduk = ref([
  { sku: 'PRD-001', name: 'Susu UHT Full Cream', brand: 'Ultra Milk', price: 1.25, stock: 45, category: 'Minuman' },
  { sku: 'PRD-002', name: 'Kopi Bubuk', brand: 'Kapal Api', price: 2.50, stock: 12, category: 'Minuman' },
  { sku: 'PRD-003', name: 'Beras Premium 5Kg', brand: 'Sania', price: 8.75, stock: 8, category: 'Sembako' },
  { sku: 'PRD-004', name: 'Minyak Goreng 2L', brand: 'Bimoli', price: 3.20, stock: 2, category: 'Sembako' },
  { sku: 'PRD-005', name: 'Mie Instan Goreng', brand: 'Indomie', price: 0.35, stock: 120, category: 'Makanan' }
])

// State untuk Pencarian dan Keranjang
const kataKunci = ref('')
const keranjang = ref([])

// Fungsi Pencarian Produk
const produkDifilter = computed(() => {
  return daftarProduk.value.filter(produk => 
    produk.name.toLowerCase().includes(kataKunci.value.toLowerCase()) ||
    produk.sku.toLowerCase().includes(kataKunci.value.toLowerCase())
  )
})

// Fungsi Keranjang Belanja
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

const totalBelanja = computed(() => {
  return keranjang.value.reduce((total, item) => total + (item.price * item.jumlah), 0)
})

const prosesCheckout = () => {
  if (keranjang.value.length === 0) return
  alert(`Transaksi Berhasil!\nTotal Pembayaran: $${totalBelanja.value.toFixed(2)}`)
  keranjang.value.forEach(itemKeranjang => {
    const produkAsli = daftarProduk.value.find(p => p.sku === itemKeranjang.sku)
    if (produkAsli) {
      produkAsli.stock -= itemKeranjang.jumlah
    }
  })
  keranjang.value = []
}
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <div class="bg-dark text-white p-3" style="width: 250px;">
      <h4 class="mb-4 text-center">Smart POS</h4>
      <ul class="nav flex-column gap-2">
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-dashboard">📊 Dashboard</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-products">📦 Kasir / Produk</RouterLink>
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
          <h2 class="fw-bold mb-0 text-dark">Mode Kasir</h2>
          <small class="text-muted">Pilih produk untuk ditambahkan ke transaksi (Draft)</small>
        </div>
        <RouterLink to="/admin-add-product" class="btn btn-outline-primary fw-bold shadow-sm">
          + Tambah Master Produk
        </RouterLink>
      </div>
      
      <div class="row">
        
        <div class="col-md-8 border-end pr-4">
          <div class="mb-4">
            <input 
              type="text" 
              class="form-control form-control-lg shadow-sm" 
              v-model="kataKunci"
              placeholder="Cari nama produk atau SKU... 🔍"
            >
          </div>
          
          <div class="row g-3">
            <div class="col-12" v-if="produkDifilter.length === 0">
              <div class="alert alert-warning text-center">Produk tidak ditemukan.</div>
            </div>

            <div class="col-md-4 col-sm-6" v-for="produk in produkDifilter" :key="produk.sku">
              <div class="card h-100 shadow-sm border-0" :class="{'opacity-50 bg-light': produk.stock <= 0}">
                <div class="card-body d-flex flex-column">
                  
                  <div class="d-flex justify-content-between align-items-start mb-2">
                    <span class="badge bg-secondary">{{ produk.category }}</span>
                    <span class="badge" :class="produk.stock > 10 ? 'bg-success' : (produk.stock > 0 ? 'bg-warning text-dark' : 'bg-danger')">
                      Stok: {{ produk.stock }}
                    </span>
                  </div>

                  <div class="d-flex justify-content-between align-items-center mb-1">
                    <small class="text-muted">{{ produk.sku }}</small>
                    <RouterLink :to="'/admin-edit-product/' + produk.sku" class="btn btn-sm btn-outline-secondary py-0 px-2" style="font-size: 0.75rem;" title="Edit Produk">
                      ✏️ Edit
                    </RouterLink>
                  </div>
                  
                  <h6 class="fw-bold mb-1">{{ produk.name }}</h6>
                  <small class="text-muted mb-3 d-block">{{ produk.brand }}</small>
                  
                  <div class="mt-auto d-flex justify-content-between align-items-center">
                    <h5 class="text-primary fw-bold mb-0">${{ produk.price.toFixed(2) }}</h5>
                    <button 
                      class="btn btn-sm btn-primary fw-bold" 
                      @click="tambahKeKeranjang(produk)"
                      :disabled="produk.stock <= 0"
                    >
                      + Add
                    </button>
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
                    <span>${{ item.price.toFixed(2) }} x {{ item.jumlah }}</span>
                    <span class="fw-bold text-dark">${{ (item.price * item.jumlah).toFixed(2) }}</span>
                  </div>
                </li>
              </ul>
            </div>
            
            <div class="card-footer bg-white border-top p-3">
              <div class="d-flex justify-content-between align-items-center mb-3">
                <h5 class="text-muted mb-0">Total</h5>
                <h3 class="text-success fw-bold mb-0">${{ totalBelanja.toFixed(2) }}</h3>
              </div>
              <button 
                class="btn btn-success btn-lg w-100 fw-bold py-3 shadow" 
                :disabled="keranjang.length === 0" 
                @click="prosesCheckout"
              >
                💵 PROSES PEMBAYARAN
              </button>
            </div>
          </div>
        </div>
        
      </div>
    </div>
  </div>
</template>