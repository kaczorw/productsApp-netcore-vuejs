<template>
  <div>
    <b-alert :show="dismissCountDown" dismissible variant="success" @dismissed="dismissCountDown=0" @dismiss-count-down="countDownChanged">
      <p class="text-center">Product added.</p>
    </b-alert>
    <b-row class="mt-3">
      <b-col>
        <b-form-group class="col-10 offset-1" label="Name:">
          <b-form-input type="text" :class="[$v.newProduct.name.$invalid ? 'is-invalid' : '']" v-model="newProduct.name" placeholder="Enter name" />
          <div class="invalid-feedback" v-if="!$v.newProduct.name.required">Field is required.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.name.minLength">Field must have at least {{ $v.newProduct.name.$params.minLength.min }} letters.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.name.maxLength">Field must have less than {{ $v.newProduct.name.$params.maxLength.max }} letters.</div>
        </b-form-group>
        <b-form-group class="col-10 offset-1" label="Hidden:">
          <b-form-checkbox v-model="newProduct.isHidden" unchecked-value="false">
            Hide product
          </b-form-checkbox>
        </b-form-group>

        <b-form-group class="col-10 offset-1" label="Description:">
          <b-form-textarea v-model="newProduct.description" :class="[$v.newProduct.description.$invalid ? 'is-invalid' : '']" placeholder="Enter description" :rows="3" :max-rows="6">
          </b-form-textarea>
          <div class="invalid-feedback" v-if="!$v.newProduct.description.required">Field is required.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.description.minLength">Field must have at least {{ $v.newProduct.description.$params.minLength.min }} letters.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.description.maxLength">Field must have less than {{ $v.newProduct.description.$params.maxLength.max }} letters.</div>
        </b-form-group>
      </b-col>
      <b-col>
        <b-form-group class="col-10 offset-1" label="Weight:">
          <b-form-input type="number" min="1" step="1" :class="[$v.newProduct.weight.$invalid ? 'is-invalid' : '']" v-model="newProduct.weight" placeholder="Enter weight" />
          <div class="invalid-feedback" v-if="!$v.newProduct.weight.required">Field is required.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.weight.minValue">Field must have at least {{ $v.newProduct.weight.$params.minValue.min }}.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.weight.maxValue">Field must have less than {{ $v.newProduct.weight.$params.maxValue.max }}.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.weight.numeric">Field must be numeric.</div>
        </b-form-group>

        <b-form-group class="col-10 offset-1" label="Price:">
          <b-form-input type="number" min="0.1" step="0.1" :class="[$v.newProduct.price.$invalid ? 'is-invalid' : '']" v-model="newProduct.price" placeholder="Enter price" />
          <div class="invalid-feedback" v-if="!$v.newProduct.price.required">Field is required.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.price.minValue">Field must have at least {{ $v.newProduct.price.$params.minValue.min }}.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.price.maxValue">Field must have less than {{ $v.newProduct.price.$params.maxValue.max }}.</div>
          <div class="invalid-feedback" v-if="!$v.newProduct.price.decimal">Field must be decimal. (in format x,xx)</div>

        </b-form-group>
        <b-form-group class="col-10 offset-1" label="Category:">
          <b-form-select v-model="newProduct.categoryId" :class="[$v.newProduct.categoryId.$invalid ? 'is-invalid' : '']">
            <option value="null" disabled>Please select category</option>
            <option v-for="category in categories" :key="category.categoryId" :value="category.categoryId">
              {{ category.name }}
            </option>
          </b-form-select>
          <div class="invalid-feedback" v-if="!$v.newProduct.categoryId.required">Field is required.</div>

        </b-form-group>
      </b-col>
    </b-row>

    <div class="col-4 offset-4 text-center">
      <b-button class="m-1" @click="resetNewProduct()" variant="danger">Reset data</b-button>
      <b-button class="m-1" @click="addProductBtn()" :disabled="$v.newProduct.$invalid" variant="success">Add</b-button>
    </div>

  </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'
import {
  required,
  numeric,
  minValue,
  maxValue,
  minLength,
  maxLength,
  helpers
} from 'vuelidate/lib/validators'

export default {
  data() {
    return {
      dismissSecs: 5,
      dismissCountDown: 0,
      newProduct: {
        categoryId: null,
        isHidden: false
      }
    }
  },
  methods: {
    ...mapActions('employee', ['addProduct', 'getProducts']),
    resetNewProduct() {
      this.newProduct = {
        categoryId: null,
        isHidden: false
      }
    },
    addProductBtn() {
      this.addProduct(this.newProduct)
        .then(() => this.getProducts())
        .then(() => this.resetNewProduct())
        .then(() => this.showAlert())
    },
    countDownChanged(dismissCountDown) {
      this.dismissCountDown = dismissCountDown
    },
    showAlert() {
      this.dismissCountDown = this.dismissSecs
    }
  },
  validations: {
    newProduct: {
      name: {
        required,
        minLength: minLength(5),
        maxLength: maxLength(30)
      },
      description: {
        required,
        minLength: minLength(5),
        maxLength: maxLength(100)
      },
      weight: {
        required,
        numeric,
        minValue: minValue(20),
        maxValue: maxValue(3000)
      },
      price: {
        required,
        decimal: helpers.regex('decimal', /^\d+(\.\d{1,2})?$/),
        minValue: minValue(0.1),
        maxValue: maxValue(1000)
      },
      categoryId: {
        required
      }
    }
  },
  computed: {
    ...mapState('employee', { categories: 'categories' })
  }
}
</script>

<style>
</style>
