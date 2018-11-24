import Vue from 'vue'
import App from './App.vue'
import store from './store'
import router from './router'
import axios from 'axios'
import moment from 'moment'
import BootstrapVue from 'bootstrap-vue'
import Vuelidate from 'vuelidate'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faCalendarAlt, faShoppingCart } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

library.add(faCalendarAlt, faShoppingCart)
Vue.component('font-awesome-icon', FontAwesomeIcon)
Vue.prototype.moment = moment
Vue.use(BootstrapVue)
Vue.use(Vuelidate)
Vue.config.productionTip = false

axios.defaults.baseURL = 'http://localhost:53184/api';

axios.interceptors.request.use(function (config) {
  if (localStorage.getItem('token')) {
    config.headers.Authorization = 'Bearer ' + localStorage.getItem('token');
  }
  return config;
}, function (error) {

  return Promise.reject(error);
});


axios.interceptors.response.use(function (response) {
  return response;
}, function (error) {
  if (!localStorage.getItem('token')
    || error.response.status === 401
    || error.response.status === 403) {
    store.commit('removeToken')
    router.push({ path: '/login' })
  }
  return Promise.reject(error);
});


new Vue({
  store,
  router,
  render: h => h(App),
}).$mount('#app')
