<template>
  <div>
    <template>
      <b-row>
        <b-col md="6" class="my-1">
          <b-form-group horizontal label="Filter" class="mb-0">
            <b-input-group>
              <b-form-input v-model="filter" placeholder="Type to Search" />
              <b-input-group-append>
                <b-btn :disabled="!filter" @click="filter = ''">Clear</b-btn>
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
        </b-col>
        <b-col md="6" class="my-1">
          <b-form-group horizontal label="Per page" class="mb-0">
            <b-form-select :options="pageOptions" v-model="perPage" />
          </b-form-group>
        </b-col>
      </b-row>

      <b-table :items="shippedOrders" :fields="fields" :filter="filter" @filtered="onFiltered" :current-page="currentPage" :per-page="perPage">
        <template slot="dateCreated" slot-scope="data">
          {{moment(data.item.dateCreated).format('Do MMMM YYYY, h:mm:ss a')}}
        </template>
        <template slot="finalPrice" slot-scope="data">
          {{data.item.finalPrice | currency}}
        </template>
        <template slot="items" slot-scope="row">
          <b-btn variant="primary" size="sm" @click="openItemsModal(row.item)" class="mr-2">
            Show items
          </b-btn>
        </template>
      </b-table>
      <b-row>
        <b-col md="6" class="my-1">
          <b-pagination :total-rows="totalRows" :per-page="perPage" v-model="currentPage" class="my-0" />
        </b-col>
      </b-row>

      <b-modal ref="shippedOrderItemsModal" size="lg" ok-only :title="moment(itemsModal.dateCreated).format('Do MMMM YYYY, h:mm:ss a')">
        <template>
          <h6>Final price: {{itemsModal.finalPrice | currency}}</h6>
          <template>
            <b-table striped hover :items="itemsModal.orderItems" :fields="itemsFields">
              <template slot="productPrice" slot-scope="row">
                {{ row.item.productPrice | currency }}
              </template>
              <template slot="price" slot-scope="row">
                {{ row.item.price | currency }}
              </template>
            </b-table>
          </template>
        </template>
      </b-modal>
    </template>
  </div>
</template>


<script>
import { mapGetters, mapActions } from 'vuex'
import currencyFilter from '../../shared/currency-filter'

export default {
  filters: {
    currency: currencyFilter
  },
  data() {
    return {
      fields: {
        orderId: {
          label: 'Order Id'
        },
        companyName: {
          label: 'Company',
          sortable: true
        },
        nip: {
          label: 'NIP',
          sortable: true
        },
        dateCreated: {
          label: 'Date',
          sortable: true
        },
        finalPrice: {
          label: 'Final Price',
          sortable: true
        },
        items: {
          label: 'Items'
        }
      },
      currentPage: 1,
      perPage: 5,
      filter: null,
      totalRows: mapGetters('employee', { shippedOrders: 'shippedOrders' })
        .length,
      pageOptions: [5, 10, 25],
      itemsModal: {},
      itemsFields: {
        productName: {
          label: 'Product name'
        },
        weight: {
          label: 'Weight [g]'
        },
        quantity: {
          label: 'Quantity'
        },
        productPrice: {
          label: 'Product price'
        },
        price: {
          label: 'Price'
        }
      }
    }
  },
  computed: {
    ...mapGetters('employee', { shippedOrders: 'shippedOrders' })
  },
  methods: {
    ...mapActions('employee', ['updateOrder', 'getOrders']),
    openItemsModal(item) {
      this.itemsModal = item
      this.$refs.shippedOrderItemsModal.show()
    },
    onFiltered(filteredItems) {
      this.totalRows = filteredItems.length
      this.currentPage = 1
    }
  }
}
</script>

<style>
</style>
