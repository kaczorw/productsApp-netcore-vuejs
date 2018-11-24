<template>
  <div id="app">
    <div v-if="loggedIn">
      <b-navbar type="dark" variant="secondary" toggleable>
        <b-navbar-toggle target="nav_dropdown_collapse"></b-navbar-toggle>
        <b-collapse is-nav id="nav_dropdown_collapse">
          <b-navbar-nav>
            <template v-if="isUser">
              <router-link class="nav-link" to="/products">Products</router-link>
              <router-link class="nav-link" to="/orders">Orders</router-link>
              <router-link v-if="cartLength >= 1" class="nav-link" to="/orders">
                <font-awesome-icon icon="shopping-cart" />
              </router-link>
            </template>
            <template v-if="isEmployee">
              <router-link class="nav-link" to="/employee-products">Products</router-link>
              <router-link class="nav-link" to="/employee-orders">Orders</router-link>
              <router-link class="nav-link" to="/employee-categories">Categories</router-link>
            </template>
          </b-navbar-nav>
          <b-navbar-nav class="ml-auto">
            <span class="nav-link">Hello: {{userName}}</span>
            <b-button variant="primary" @click="logout()">Logout</b-button>
          </b-navbar-nav>
        </b-collapse>
      </b-navbar>
    </div>
    <b-container fluid>
      <router-view />
    </b-container>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex'
export default {
  name: 'app',
  computed: {
    ...mapGetters(['loggedIn', 'isEmployee', 'isUser', 'userName']),
    ...mapGetters('user', { cartLength: 'cartLength' })
  },
  methods: {
    ...mapMutations(['removeToken']),
    logout() {
      this.removeToken()
      this.$router.push({ path: '/login' })
    }
  }
}
</script>

<style>
.router-link-active {
  color: white !important;
}
</style>
