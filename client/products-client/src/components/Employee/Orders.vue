<template>
  <div>
    <h2>Orders</h2>

    <b-tabs class="mt-3">
      <b-tab title="New orders" active>
        <NewOrders />
      </b-tab>
      <b-tab title="Shipped orders">
        <ShippedOrders />
      </b-tab>
    </b-tabs>
  </div>

</template>

<script>
import NewOrders from './NewOrders.vue'
import ShippedOrders from './ShippedOrders.vue'

import { mapActions } from 'vuex'

export default {
  components: { NewOrders, ShippedOrders },
  methods: {
    ...mapActions('employee', ['getOrders'])
  },
  created() {
    this.getOrders()
  }
}
</script>

<style>
</style>
