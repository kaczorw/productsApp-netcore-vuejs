import axios from 'axios'

export default {
  namespaced: true,
  state: {
    products: [],
    cart: [],
    orders: []

  },
  mutations: {
    addProductToCart(state, item) {
      let same
      state.cart.forEach(element => {
        if (item.product.productId === element.product.productId) {
          element.quantity = +element.quantity + +item.quantity
          same = true
        }
      })
      if (!same) {
        state.cart.push(item)
      }
    },
    deleteCartItem(state, item) {
      let index = state.cart.indexOf(item)
      state.cart.splice(index, 1)
    },
    changeQuantity(state, item) {
      let index = state.cart.indexOf(item.product)
      let product = state.cart[index]
      product.quantity = item.newQuantity
    },
    clearCart(state) {
      state.cart = []
    },
    updateProducts(state, products) {
      state.products = products

    },
    updateOrders(state, orders) {
      state.orders = orders
    }
  },
  actions: {
    getProducts({ commit }) {
      axios.get('/products/foruser')
        .then(result => commit('updateProducts', result.data))
    },
    getOrders({ commit }) {
      axios.get('/orders/userid')
        .then(result => commit('updateOrders', result.data))
    },
    sendOrder({ state }) {
      let orderItems = []
      state.cart.forEach(element => {
        orderItems.push({ "productId": element.product.productId, "quantity": element.quantity })
      })
      return axios.post('/orders', { orderItems: orderItems })
    }
  },
  getters: {
    cartLength(state) {
      return state.cart.length
    },
    cartFinalPrice(state) {
      let sum = 0;
      state.cart.forEach(element => {
        sum += element.product.price * element.quantity
      });
      return sum
    },
    newOrders(state) {
      return state.orders.filter(x => x.status === "New")
    },
    shippedOrders(state) {
      return state.orders.filter(x => x.status === "Shipped")
    }
  }
}
