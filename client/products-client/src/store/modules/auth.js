import axios from 'axios'

export default {
  state: {
    token: localStorage.getItem('token') || null,
  },
  mutations: {
    saveToken(state, token) {
      localStorage.setItem('token', token)
      state.token = token
    },
    removeToken(state) {
      localStorage.removeItem('token')
      state.token = null
    }
  },
  actions: {
    login({ commit }, credentials) {
      return axios.post('/auth/token', credentials)
        .then(result => commit('saveToken', result.data.token))
    }
  },
  getters: {
    loggedIn(state) {
      return state.token !== null
    },
    isUser(state) {
      if (state.token !== null) {
        let parsedToken = parseJwt(state.token)
        if (parsedToken.role === "user") {
          return true
        }
      }
      return false
    },
    isEmployee(state) {
      if (state.token !== null) {
        let parsedToken = parseJwt(state.token)
        if (parsedToken.role === "employee") {
          return true
        }
      }
      return false
    },
    userName(state) {
      if (state.token !== null) {
        let parsedToken = parseJwt(state.token)
        return parsedToken.unique_name
      }
    }
  }
}

function parseJwt(token) {
  let base64Url = token.split('.')[1];
  let base64 = base64Url.replace('-', '+').replace('_', '/');
  return JSON.parse(window.atob(base64));
}
