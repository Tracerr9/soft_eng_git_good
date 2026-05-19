<script setup>
import { ref } from 'vue'

const daftarKaryawan = ref([
  { id: '1001', nama: 'Budi Santoso', role: 'Kasir', status: 'Aktif' },
  { id: '1002', nama: 'Siti Aminah', role: 'Kasir', status: 'Aktif' },
  { id: '1003', nama: 'Andi Wijaya', role: 'Admin', status: 'Cuti' }
])
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
          <RouterLink class="nav-link text-white" to="/admin-products">📦 Kelola Produk</RouterLink>
        </li>
         <li class="nav-item">
          <RouterLink class="nav-link text-white" to="/admin-transactions">🧾 Riwayat Transaksi</RouterLink>
        </li>
        <li class="nav-item">
          <RouterLink class="nav-link text-white bg-primary rounded" to="/admin-employee">👥 Kelola Karyawan</RouterLink>
        </li>
        <li class="nav-item mt-5">
          <RouterLink class="nav-link text-danger" to="/">🚪 Logout</RouterLink>
        </li>
      </ul>
    </div>

    <div class="p-4 flex-grow-1 bg-light">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Kelola Karyawan</h2>
        <RouterLink to="/admin-add-employee" class="btn btn-success fw-bold">
          + Tambah Karyawan
        </RouterLink>
      </div>

      <div class="card shadow-sm">
        <div class="card-body">
          <table class="table table-hover align-middle">
            <thead class="table-dark">
              <tr>
                <th>ID</th>
                <th>Nama Karyawan</th>
                <th>Posisi</th>
                <th>Status</th>
                <th>Aksi</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="emp in daftarKaryawan" :key="emp.id">
                <td class="fw-bold">{{ emp.id }}</td>
                <td>{{ emp.nama }}</td>
                <td><span class="badge bg-secondary">{{ emp.role }}</span></td>
                <td>
                  <span :class="{'text-success fw-bold': emp.status === 'Aktif', 'text-warning fw-bold': emp.status === 'Cuti'}">
                    {{ emp.status }}
                  </span>
                </td>
                <td>
                  <RouterLink :to="'/admin-edit-employee/' + emp.id" class="btn btn-sm btn-outline-primary me-2 fw-bold">
                    Edit
                  </RouterLink>
                  <button class="btn btn-sm btn-outline-danger fw-bold">Hapus</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      
    </div>
  </div>
</template>