<template>
  <div>
    <b-alert :show="dismissCountDown" dismissible variant="success" @dismissed="dismissCountDown=0" @dismiss-count-down="countDownChanged">
      <p class="text-center">Order placed.</p>
    </b-alert>
    <div v-if="cartFinalPrice > 0">
      <template>
        <b-table striped hover :items="newOrder" :fields="fields">
          <template slot="quantity" slot-scope="row">
            <input type="number" min="1" step="1" class="form-control" v-on:input="calculatePrice(row.item, $event)" placeholder="Enter quantity" :value="row.item.quantity">
          </template>
          <template slot="product.price" slot-scope="row">
            {{row.item.product.price | currency}}
          </template>
          <template slot="price" slot-scope="row">
            {{(row.item.product.price * row.item.quantity) | currency}}
          </template>
          <template slot="deleteItem" slot-scope="row">
            <b-btn @click="deleteCartItemBtn(row.item)" variant="danger">Delete</b-btn>
          </template>
        </b-table>
      </template>
      <div class="col-4 offset-4 text-center">
        <h5>Final price: {{ cartFinalPrice | currency }}</h5>
        <b-btn v-b-modal.sendOrder variant="success">Send</b-btn>
      </div>
      <b-modal id="sendOrder" title="Confirmation" ok-title="Send" ok-variant="success" @ok="sendOrderBtn()">
        <p>Are you sure you want to send this order?</p>
      </b-modal>
    </div>
    <div v-else>
      <br>
      <h6>Your cart is empty.</h6>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapActions, mapState } from 'vuex'
import currencyFilter from '../../shared/currency-filter'

export default {
  filters: {
    currency: currencyFilter
  },
  data() {
    return {
      dismissSecs: 5,
      dismissCountDown: 0,
      fields: {
        'product.name': {
          label: 'Product name'
        },
        'product.weight': {
          label: 'Weight [g]'
        },
        quantity: {
          label: 'Quantity'
        },
        'product.price': {
          label: 'Product price'
        },
        price: {
          label: 'Price'
        },
        deleteItem: {
          label: 'Delete'
        }
      }
    }
  },
  methods: {
    ...mapMutations('user', ['deleteCartItem', 'clearCart', 'changeQuantity']),
    ...mapActions('user', ['getOrders', 'sendOrder']),
    deleteCartItemBtn(item) {
      this.deleteCartItem(item)
    },
    sendOrderBtn() {
      this.sendOrder()
        .then(() => this.getOrders())
        .then(() => this.clearCart())
        .then(() => this.showAlert())
    },
    calculatePrice(item, event) {
      if (event.target.value < 0) {
        this.deleteCartItem(item)
        return
      } else if (event.target.value == 0) {
        event.target.value = 1
      } else if (!/^\+?(0|[1-9]\d*)$/.test(event.target.value)) {
        event.target.value = 1
      } else if (event.target.value > 1000) {
        event.target.value = 1000
      }
      let payload = {
        product: item,
        newQuantity: event.target.value
      }
      this.changeQuantity(payload)
    },
    countDownChanged(dismissCountDown) {
      this.dismissCountDown = dismissCountDown
    },
    showAlert() {
      this.dismissCountDown = this.dismissSecs
    }
  },
  computed: {
    ...mapState('user', { newOrder: 'cart' }),
    ...mapGetters('user', { cartFinalPrice: 'cartFinalPrice' })
  }
}
</script>

<style>
</style>
