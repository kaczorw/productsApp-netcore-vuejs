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

        <b-table :items="availableProducts" :fields="fields" :filter="filter" @filtered="onFiltered" :current-page="currentPage" :per-page="perPage">
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
          <template slot="edit" slot-scope="row">
            <b-btn variant="warning" size="sm" @click="openEditProductModal(row.item)" class="mr-2">
              Edit
            </b-btn>
          </template>
        </b-table>

        <b-row>
          <b-col md="6" class="my-1">
            <b-pagination :total-rows="totalRows" :per-page="perPage" v-model="currentPage" class="my-0" />
          </b-col>
        </b-row>

        <b-modal :ok-disabled="$v.editProductModal.$invalid" ref="editAvailableProductModal" ok-title="Edit" ok-variant="warning" :title="editProductModal.name" @ok="editProductBtn()">
          <b-row>
            <b-form-group class="col-10 offset-1" label="Name:">
              <b-form-input type="text" :class="[$v.editProductModal.name.$invalid ? 'is-invalid' : '']" v-model="editProductModal.name" placeholder="Enter name" />
              <div class="invalid-feedback" v-if="!$v.editProductModal.name.required">Field is required.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.name.minLength">Field must have at least {{ $v.editProductModal.name.$params.minLength.min }} letters.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.name.maxLength">Field must have less than {{ $v.editProductModal.name.$params.maxLength.max }} letters.</div>
            </b-form-group>
            <b-form-group class="col-10 offset-1" label="Hidden:">
              <b-form-checkbox v-model="editProductModal.isHidden" unchecked-value="false">
                Hide product
              </b-form-checkbox>
            </b-form-group>

            <b-form-group class="col-10 offset-1" label="Description:">
              <b-form-textarea v-model="editProductModal.description" :class="[$v.editProductModal.description.$invalid ? 'is-invalid' : '']" placeholder="Enter description" :rows="3" :max-rows="6">
              </b-form-textarea>
              <div class="invalid-feedback" v-if="!$v.editProductModal.description.required">Field is required.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.description.minLength">Field must have at least {{ $v.editProductModal.description.$params.minLength.min }} letters.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.description.maxLength">Field must have less than {{ $v.editProductModal.description.$params.maxLength.max }} letters.</div>
            </b-form-group>

            <b-form-group class="col-10 offset-1" label="Weight:">
              <b-form-input type="number" min="1" step="1" :class="[$v.editProductModal.weight.$invalid ? 'is-invalid' : '']" v-model="editProductModal.weight" placeholder="Enter weight" />
              <div class="invalid-feedback" v-if="!$v.editProductModal.weight.required">Field is required.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.weight.minValue">Field must have at least {{ $v.editProductModal.weight.$params.minValue.min }}.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.weight.maxValue">Field must have less than {{ $v.editProductModal.weight.$params.maxValue.max }}.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.weight.numeric">Field must be numeric.</div>
            </b-form-group>

            <b-form-group class="col-10 offset-1" label="Price:">
              <b-form-input type="number" min="0.1" step="0.1" :class="[$v.editProductModal.price.$invalid ? 'is-invalid' : '']" v-model="editProductModal.price" placeholder="Enter price" />
              <div class="invalid-feedback" v-if="!$v.editProductModal.price.required">Field is required.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.price.minValue">Field must have at least {{ $v.editProductModal.price.$params.minValue.min }}.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.price.maxValue">Field must have less than {{ $v.editProductModal.price.$params.maxValue.max }}.</div>
              <div class="invalid-feedback" v-if="!$v.editProductModal.price.decimal">Field must be decimal. (in format x,xx)</div>

            </b-form-group>
            <b-form-group class="col-10 offset-1" label="Category:">
              <b-form-select v-model="editProductModal.categoryId" :class="[$v.editProductModal.categoryId.$invalid ? 'is-invalid' : '']">
                <option value="null" disabled>Please select category</option>
                <option v-for="category in categories" :key="category.categoryId" :value="category.categoryId">
                  {{ category.name }}
                </option>
              </b-form-select>
              <div class="invalid-feedback" v-if="!$v.editProductModal.categoryId.required">Field is required.</div>
            </b-form-group>
          </b-row>
        </b-modal>
      </b-container>
    </template>
  </div>
</template>

<script>
import currencyFilter from '../../shared/currency-filter'
import { mapGetters, mapState, mapActions } from 'vuex'
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
  filters: {
    currency: currencyFilter
  },
  data() {
    return {
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
        edit: {
          label: 'Edit'
        }
      },
      currentPage: 1,
      perPage: 5,
      totalRows: mapGetters('employee', {
        availableProducts: 'availableProducts'
      }).length,
      pageOptions: [5, 10, 25],
      filter: null,
      editProductModal: {}
    }
  },
  validations: {
    editProductModal: {
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
  methods: {
    ...mapActions('employee', ['editProduct', 'getProducts']),
    onFiltered(filteredItems) {
      this.totalRows = filteredItems.length
      this.currentPage = 1
    },
    openEditProductModal(item) {
      this.editProductModal = JSON.parse(JSON.stringify(item))
      this.$refs.editAvailableProductModal.show()
    },
    editProductBtn() {
      this.editProduct(this.editProductModal).then(() => this.getProducts())
    }
  },
  computed: {
    ...mapGetters('employee', { availableProducts: 'availableProducts' }),
    ...mapState('employee', { categories: 'categories' })
  }
}
</script>

<style>
</style>

