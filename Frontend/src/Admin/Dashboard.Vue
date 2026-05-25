<script setup>
import { ref } from 'vue'

const jumlahProduk = ref(145)
const jumlahKaryawan = ref(12)
const transaksiHariIni = ref(85)
</script>

<template>
  <div class="d-flex" style="min-height: 100vh;">
    <div class="bg-dark text-white p-3" style="width: 250px;">
      <h4 class="mb-4 text-center">Smart POS</h4>
      <ul class="nav flex-column gap-2">
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-notifications">Notifikasi System</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-dashboard">Dashboard</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/product">Produk</RouterLink>
        </li>
         <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-transactions">Riwayat Transaksi</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-employee">Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">Logout</RouterLink>
        </li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 bg-light">
      <h2 class="mb-4">Dashboard Admin</h2>

      <div class="row mb-4">
        <div class="col-md-4">
          <div class="card text-white bg-info mb-3 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">Total Produk</h5>
              <h2 class="card-text">{{ jumlahProduk }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card text-white bg-success mb-3 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">Transaksi Hari Ini</h5>
              <h2 class="card-text">{{ transaksiHariIni }}</h2>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card text-white bg-warning mb-3 shadow-sm">
            <div class="card-body">
              <h5 class="card-title">Total Karyawan</h5>
              <h2 class="card-text">{{ jumlahKaryawan }}</h2>
            </div>
          </div>
        </div>
      </div>

      <div class="card shadow-sm">
        <div class="card-header bg-white">
          <h5 class="mb-0 text-danger fw-bold">⚠️ Peringatan Stok Menipis</h5>
        </div>
        <div class="card-body">
          <table class="table table-hover align-middle">
            <thead class="table-light">
              <tr>
                <th>ID</th>
                <th>Nama Barang</th>
                <th>Sisa Stok</th>
                <th>Aksi</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>PRD-001</td>
                <td class="fw-bold">Minyak Goreng 2L</td>
                <td class="text-danger fw-bold">3</td>
                <td><RouterLink class="btn btn-sm btn-primary" to="/product">Restock</RouterLink></td>
              </tr>
              <tr>
                <td>PRD-002</td>
                <td class="fw-bold">Beras Setra Ramos 5kg</td>
                <td class="text-danger fw-bold">1</td>
                <td><RouterLink class="btn btn-sm btn-primary" to="/product">Restock</RouterLink></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

    </div>
  </div>
</template>