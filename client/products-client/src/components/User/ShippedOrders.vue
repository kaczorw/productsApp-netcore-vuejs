<template>
  <div>
    <template>
      <b-row>
        <b-col md="6" class="my-1">
          <b-form-group horizontal label="Filter" class="mb-0">
            <b-input-group>
              <b-input-group-prepend>
                <b-btn @click="openDataPicker">
                  <font-awesome-icon icon="calendar-alt" />
                </b-btn>
              </b-input-group-prepend>
              <datepicker ref="programaticOpen" v-model="selectedDate" placeholder="Select Date" :bootstrap-styling="true" />
              <b-input-group-append>
                <b-btn :disabled="!selectedDate" @click="selectedDate = ''">Clear</b-btn>
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
      <b-table :items="shippedOrders" :fields="fields" :filter="filteredDate" @filtered="onFiltered" :current-page="currentPage" :per-page="perPage">
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
              </template> <template slot="price" slot-scope="row">
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
import { mapGetters } from 'vuex'
import currencyFilter from '../../shared/currency-filter'
import Datepicker from 'vuejs-datepicker'

export default {
  components: {
    Datepicker
  },
  filters: {
    currency: currencyFilter
  },
  data() {
    return {
      selectedDate: '',
      fields: {
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
      totalRows: mapGetters('user', { getShippedOrders: 'getShippedOrders' })
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
    ...mapGetters('user', { shippedOrders: 'shippedOrders' }),
    filteredDate: {
      get() {
        if (
          Object.prototype.toString.call(this.selectedDate) === '[object Date]'
        ) {
          let day = this.selectedDate.getDate()
          if (day < 10) {
            day = '0' + day
          }
          let month = this.selectedDate.getMonth() + 1
          if (month < 10) {
            month = '0' + month
          }
          let year = this.selectedDate.getFullYear()
          return `${year}-${month}-${day}`
        }
      }
    }
  },
  methods: {
    openItemsModal(item) {
      this.itemsModal = item
      this.$refs.shippedOrderItemsModal.show()
    },
    onFiltered(filteredItems) {
      this.totalRows = filteredItems.length
      this.currentPage = 1
    },
    openDataPicker() {
      this.$refs.programaticOpen.showCalendar()
    }
  }
}
</script>

<style>
</style>
