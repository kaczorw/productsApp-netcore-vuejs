<template>
  <div class="col-6 offset-3" style="margin-top:150px;">
    <b-form @submit.prevent="loginForm">
      <b-form-group label="Username:">
        <b-form-input @input="$v.credentials.username.$touch()" type="text" v-model="credentials.username" :class="[$v.credentials.username.$invalid && $v.credentials.username.$dirty ? 'is-invalid' : '']" placeholder="Enter username" />
        <div class="invalid-feedback" v-if="!$v.credentials.username.required">Field is required.</div>
      </b-form-group>
      <b-form-group label="Password:">
        <b-form-input type="password" @input="$v.credentials.password.$touch()" v-model="credentials.password" :class="[$v.credentials.password.$invalid && $v.credentials.password.$dirty ? 'is-invalid' : '']" placeholder="Enter password" />
        <div class="invalid-feedback" v-if="!$v.credentials.password.required">Field is required.</div>
      </b-form-group>
      <div class="text-center">
        <b-button type="submit" :disabled="$v.credentials.$invalid" variant="primary">Login</b-button>
      </div>
    </b-form>
    <b-alert class="my-2" :show="invalidCredentialsAlert" variant="danger">Invalid credentials. Try again.</b-alert>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import { required } from 'vuelidate/lib/validators'

export default {
  data() {
    return {
      credentials: {
        username: '',
        password: ''
      },
      invalidCredentialsAlert: false
    }
  },
  validations: {
    credentials: {
      username: {
        required
      },
      password: {
        required
      }
    }
  },
  computed: mapGetters(['loggedIn', 'isUser', 'isEmployee']),
  methods: {
    ...mapActions(['login']),
    loginForm() {
      this.login(this.credentials)
        .then(() => {
          if (this.isUser) {
            this.$router.push({ path: '/products' })
          } else if (this.isEmployee) {
            this.$router.push({ path: '/employee-products' })
          }
        })
        .catch(() => (this.invalidCredentialsAlert = true))
    }
  },
  created() {
    if (this.isUser) {
      this.$router.push({ path: '/products' })
    } else if (this.isEmployee) {
      this.$router.push({ path: '/employee-products' })
    }
  }
}
</script>

<style>
</style>
