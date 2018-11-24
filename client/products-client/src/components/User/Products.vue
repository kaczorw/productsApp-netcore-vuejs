<template>
  <div>
    <template>
      <b-container fluid>
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

        <b-table :items="products" :fields="fields" :filter="filter" @filtered="onFiltered" :current-page="currentPage" :per-page="perPage">
          <template slot="price" slot-scope="data">
            {{data.item.price | currency}}
          </template>
          <template slot="showDescription" slot-scope="row">
            <b-button size="sm" @click="row.toggleDetails" class="mr-2">
              {{ row.detailsShowing ? 'Hide' : 'Show'}} Description
            </b-button>
          </template>
          <template slot="row-details" slot-scope="row">
            <b-card>
              <b-row class="mb-2">
                <b-col sm="3" class="text-sm-right"><b>Description:</b></b-col>
                <b-col>{{ row.item.description }}</b-col>
              </b-row>
            </b-card>
          </template>
          <template slot="addToCard" slot-scope="row">
            <b-btn variant="primary" size="sm" @click="openAddToCardModal(row.item)" class="mr-2">
              Add to card
            </b-btn>
          </template>
        </b-table>

        <b-row>
          <b-col md="6" class="my-1">
            <b-pagination :total-rows="totalRows" :per-page="perPage" v-model="currentPage" class="my-0" />
          </b-col>
        </b-row>

        <b-modal ref="addToCardModal" @hide="resetQuantity" :title="addToCardModal.name" ok-title="Add" ok-variant="success" :ok-disabled="$v.quantity.$invalid" @ok="addToCard(addToCardModal, quantity)">
          <b-row>
            <b-col>
              <h5>Price: {{addToCardModal.price | currency}}</h5>
              <h5>Weight: {{addToCardModal.weight | weight}}</h5>
            </b-col>
          </b-row>

          <div class="py-1">

            <b-card>
              <p class="card-text">{{addToCardModal.description}}</p>
            </b-card>

          </div>
          <div>
            <b-form-input type="number" min="1" step="1" placeholder="Enter quantity" v-model="quantity" :state="!$v.quantity.$invalid"></b-form-input>
            <b-form-invalid-feedback>
              This is a required field and must be numeric.
            </b-form-invalid-feedback>
            <span v-if="quantity >= 1">Calculated price: {{addToCardModal.price | currency}} * {{quantity}} = {{(quantity * addToCardModal.price) | currency}} </span>

          </div>
        </b-modal>

      </b-container>
    </template>
  </div>
</template>

<script>
import currencyFilter from '../../shared/currency-filter'
import weightFilter from '../../shared/weight-filter'
import { required, numeric, minValue, maxValue } from 'vuelidate/lib/validators'
import { mapState, mapMutations, mapActions } from 'vuex'

export default {
  filters: {
    currency: currencyFilter,
    weight: weightFilter
  },
  data() {
    return {
      quantity: '',
      fields: {
        name: {
          label: 'Name',
          sortable: true
        },
        weight: {
          label: 'Weight [g]',
          sortable: true
        },
        price: {
          label: 'Price',
          sortable: true
        },
        category: {
          label: 'Category',
          sortable: true
        },
        showDescription: {
          label: 'Show Description'
        },
        addToCard: {
          label: 'Add to card'
        }
      },
      currentPage: 1,
      perPage: 5,
      //totalRows: this.$store.state.products.length,
      totalRows: mapState('user', { products: 'products' }).length,
      pageOptions: [5, 10, 25],
      filter: null,
      addToCardModal: {}
    }
  },
  methods: {
    ...mapActions('user', ['getProducts']),
    ...mapMutations('user', ['addProductToCart']),
    openAddToCardModal(item) {
      this.addToCardModal = item
      this.$refs.addToCardModal.show()
      //this.$root.$emit('bv::show::modal', 'addToCardModal', button)
    },
    resetQuantity() {
      this.quantity = ''
    },
    addToCard(product, quantity) {
      let item = { product: product, quantity: quantity }
      this.addProductToCart(item)
    },
    onFiltered(filteredItems) {
      this.totalRows = filteredItems.length
      this.currentPage = 1
    }
  },
  validations: {
    quantity: {
      required,
      numeric,
      minValue: minValue(1),
      maxValue: maxValue(1000)
    }
  },
  computed: {
    ...mapState('user', { products: 'products' })
  },
  created() {
    this.getProducts()
  }
}
</script>

<style>
</style>
