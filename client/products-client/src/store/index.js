import Vue from 'vue'
import Vuex from 'vuex'
import authModule from './modules/auth'
import userModule from './modules/user'
import employeeModule from './modules/employee'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    auth: authModule,
    user: userModule,
    employee: employeeModule
  }
})

