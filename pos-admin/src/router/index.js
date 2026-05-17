import { createRouter, createWebHistory } from 'vue-router'
import AdminView from '../Admin/Dashboard.vue'
import EmployeeView from '../Admin/Employee.vue'
import ProductsView from '../Admin/Products.vue'
import AuthView from '../Login/Login.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/admin-dashboard',
      name: 'dashboard',
      component: AdminView,
    },
    {
      path: '/admin-employee',
      name: 'employee',
      component: EmployeeView,
    },
    {
      path: '/admin-products',
      name: 'products',
      component: ProductsView,
    },
    {
      path: '/',
      name: 'login',
      component: AuthView,
    },
  ],
})

export default router
