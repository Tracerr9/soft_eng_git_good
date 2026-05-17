<script setup>
import { ref, computed } from 'vue'

const daftarProduk = ref([
  { id: 1, nama: 'Pasta Penne', merk: 'Panzati', hargaSatuan: 1.35, hargaBox: 40.5, isiBox: 30, rak: 30, gudang: 0, box: 0 },
  { id: 2, nama: 'Water Bottle', merk: 'Montcuq', hargaSatuan: 0.89, hargaBox: 26.7, isiBox: 30, rak: 30, gudang: 0, box: 0 },
  { id: 3, nama: 'Rice', merk: 'Elo', hargaSatuan: 1.45, hargaBox: 43.5, isiBox: 30, rak: 30, gudang: 0, box: 0 },
  { id: 4, nama: 'Salt', merk: 'Labalaine', hargaSatuan: 0.45, hargaBox: 22.5, isiBox: 50, rak: 50, gudang: 0, box: 0 }
])

const keranjang = ref([])

const tambahKeKeranjang = (produk) => {
  const itemAda = keranjang.value.find(item => item.id === produk.id)
  if (itemAda) {
    itemAda.jumlah++
  } else {
    keranjang.value.push({ ...produk, jumlah: 1 })
  }
}

const totalBelanja = computed(() => {
  return keranjang.value.reduce((total, item) => total + (item.hargaBox * item.jumlah), 0)
})

const checkout = () => {
  alert('Pembelian senilai $' + totalBelanja.value.toFixed(2) + ' berhasil!')
  keranjang.value = [] // Kosongkan keranjang setelah beli
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
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-products">📦 Kelola Produk</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-employee">👥 Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">🚪 Logout</RouterLink>
        </li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 text-white" style="background-color: #2c2c2c;">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="fw-bold">Manager Blackboard</h2>
        <div class="d-flex gap-3 align-items-center">
          <span class="badge bg-danger">Rak</span>
          <span class="badge bg-success">Gudang</span>
          <span class="badge bg-warning text-dark">Box</span>
        </div>
      </div>

      <div class="row">
        <div class="col-md-8">
          <input type="text" class="form-control mb-3 w-50 bg-dark text-white border-secondary" placeholder="Cari produk... 🔍">
          
          <div class="row g-3">
            <div class="col-md-6" v-for="produk in daftarProduk" :key="produk.id">
              <div class="card bg-info text-dark shadow-sm h-100" style="background-color: #89c4f4 !important;">
                <div class="card-body d-flex p-2">
                  <div class="d-flex flex-column gap-1 me-2" style="width: 30px;">
                    <div class="bg-danger text-white text-center rounded small fw-bold">{{ produk.rak }}</div>
                    <div class="bg-success text-white text-center rounded small fw-bold">{{ produk.gudang }}</div>
                    <div class="bg-warning text-dark text-center rounded small fw-bold">{{ produk.box }}</div>
                  </div>
                  <div class="flex-grow-1">
                    <h6 class="fw-bold mb-0">{{ produk.nama }}</h6>
                    <small class="text-muted d-block" style="font-size: 0.75rem;">{{ produk.merk }}</small>
                    <small class="d-block mt-1">Price/unit ${{ produk.hargaSatuan }}</small>
                    <div class="d-flex justify-content-between align-items-end mt-2">
                      <span class="small fw-bold">x{{ produk.isiBox }}</span>
                      <h5 class="fw-bold mb-0">${{ produk.hargaBox }}</h5>
                    </div>
                  </div>
                  <div class="d-flex align-items-end ms-2">
                    <button class="btn btn-light btn-sm fw-bold border" @click="tambahKeKeranjang(produk)">+</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="col-md-4">
          <div class="card bg-light h-100 shadow">
            <div class="card-header bg-white">
              <h4 class="mb-0 fw-bold text-dark">Shopping List</h4>
            </div>
            <div class="card-body overflow-auto" style="max-height: 60vh;">
              <p v-if="keranjang.length === 0" class="text-muted text-center mt-5">Belum ada pesanan restock.</p>
              <ul class="list-group list-group-flush" v-else>
                <li class="list-group-item d-flex justify-content-between align-items-center bg-transparent px-0" v-for="(item, index) in keranjang" :key="index">
                  <div>
                    <span class="fw-bold">{{ item.nama }}</span>
                    <small class="d-block text-muted">{{ item.jumlah }} Box x ${{ item.hargaBox }}</small>
                  </div>
                  <span class="fw-bold text-dark">${{ (item.hargaBox * item.jumlah).toFixed(2) }}</span>
                </li>
              </ul>
            </div>
            <div class="card-footer bg-dark text-white d-flex justify-content-between align-items-center p-3">
              <h3 class="mb-0 fw-bold">${{ totalBelanja.toFixed(2) }}</h3>
              <button class="btn btn-light btn-lg fw-bold px-4" :disabled="keranjang.length === 0" @click="checkout">Buy</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>