import store from '../store'

export default (to, from, next) => {
  if (store.getters.loggedIn && store.getters.isEmployee) {
    next()
  } else {
    next('/login')
  }
}