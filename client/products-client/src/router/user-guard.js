import store from '../store'

export default (to, from, next) => {
  if (store.getters.loggedIn && store.getters.isUser) {
    next()
  } else {
    next('/login')
  }
}