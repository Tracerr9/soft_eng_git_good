import { createRouter, createWebHistory } from 'vue-router'
import AdminView from '../Admin/Dashboard.vue'
import EmployeeView from '../Admin/Employee.vue'
import ProductsView from '../Admin/Products.vue'
import AuthView from '../Login/Login.vue'
import AddProdView from '../Admin/AddProduct.vue'
import AddTransactions from '../Admin/Transactions.vue'
import AddTransactionDetail from '../Admin/TransactionDetails.vue'
import AddEmployee from '../Admin/AddEmployee.vue'
import AddEditEmployee from '../Admin/EditEmployee.vue'
import EditProductView from '../Admin/EditProduct.vue'
import NotifView from '../Admin/Notification.vue'

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
      path: '/product',
      name: 'product',
      component: ProductsView,
    },
    {
      path: '/product/:sku', 
      name: 'edit-product',
      component: EditProductView,
    },
    {
      path: '/',
      name: 'login',
      component: AuthView,
    },
    {
      path: '/admin-add-product',
      name: 'add-products',
      component: AddProdView,
    },
    {
      path: '/admin-transactions',
      name: 'transactions',
      component: AddTransactions,
    },
    {
      path: '/admin-transaction/:id', 
      name: 'transactionDetail',
      component: AddTransactionDetail,
    },
    {
      path: '/admin-add-employee', 
      name: 'add-employee',
      component: AddEmployee,
    },
    {
      path: '/admin-edit-employee/:id', 
      name: 'edit-employee',
      component: AddEditEmployee,
    },
    {
      path: '/notification', 
      name: 'notification',
      component: AddEditEmployee,
    },
    {
      path: '/admin-notifications', 
      name: 'notifications',
      component: NotifView,
    },
  ],
})

export default router