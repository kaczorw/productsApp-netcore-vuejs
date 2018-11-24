import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

import Login from '../components/Login.vue'
import Products from '../components/User/Products.vue'
import Orders from '../components/User/Orders.vue'
import EmployeeProducts from '../components/Employee/Products.vue'
import EmployeeOrders from '../components/Employee/Orders.vue'
import Categories from '../components/Employee/Categories.vue'
import UserGuard from './user-guard'
import EmployeeGuard from './employee-guard'

export default new VueRouter({
  mode: 'history',
  routes: [{
    path: '/login',
    name: 'login',
    component: Login
  },
  //user
  {
    path: '/products',
    name: 'products',
    component: Products,
    beforeEnter: UserGuard
  },
  {
    path: '/orders',
    name: 'orders',
    component: Orders,
    beforeEnter: UserGuard
  },
  //employee
  {
    path: '/employee-products',
    name: 'employee-products',
    component: EmployeeProducts,
    beforeEnter: EmployeeGuard
  },
  {
    path: '/employee-orders',
    name: 'employee-orders',
    component: EmployeeOrders,
    beforeEnter: EmployeeGuard
  },
  {
    path: '/employee-categories',
    name: 'employee-categories',
    component: Categories,
    beforeEnter: EmployeeGuard
  },
  {
    path: '*',
    redirect: { path: '/login' }
  }

  ]
})