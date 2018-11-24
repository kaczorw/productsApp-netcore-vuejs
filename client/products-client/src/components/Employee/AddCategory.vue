<template>
  <div>
    <b-alert :show="dismissCountDown" dismissible variant="success" @dismissed="dismissCountDown=0" @dismiss-count-down="countDownChanged">
      <p class="text-center">Category added.</p>
    </b-alert>
    <b-form-group class="col-10 offset-1 mt-3" label="Name:">
      <b-form-input type="text" :class="[$v.newCategory.name.$invalid ? 'is-invalid' : '']" v-model="newCategory.name" placeholder="Enter name" />
      <div class="invalid-feedback" v-if="!$v.newCategory.name.required">Field is required.</div>
      <div class="invalid-feedback" v-if="!$v.newCategory.name.minLength">Field must have at least {{ $v.newCategory.name.$params.minLength.min }} letters.</div>
      <div class="invalid-feedback" v-if="!$v.newCategory.name.maxLength">Field must have less than {{ $v.newCategory.name.$params.maxLength.max }} letters.</div>
    </b-form-group>
    <div class="col-4 offset-4 text-center">
      <b-button @click="addCategoryBtn()" :disabled="$v.newCategory.name.$invalid" variant="success">Add</b-button>
    </div>
  </div>
</template>

<script>
import { mapActions } from 'vuex'
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

export default {
  data() {
    return {
      dismissSecs: 5,
      dismissCountDown: 0,
      newCategory: {}
    }
  },
  methods: {
    ...mapActions('employee', ['addCategory', 'getCategories']),
    countDownChanged(dismissCountDown) {
      this.dismissCountDown = dismissCountDown
    },
    showAlert() {
      this.dismissCountDown = this.dismissSecs
    },
    addCategoryBtn() {
      this.addCategory(this.newCategory)
        .then(() => this.getCategories())
        .then(() => (this.newCategory = {}))
        .then(() => this.showAlert())
    }
  },
  validations: {
    newCategory: {
      name: {
        required,
        minLength: minLength(5),
        maxLength: maxLength(20)
      }
    }
  }
}
</script>

<style>
</style>
