<template>
  <div>
    <h2>Categories</h2>

    <b-tabs class="mt-3">
      <b-tab title="Categories" active>

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

            <b-table :items="categories" :fields="fields" :filter="filter" @filtered="onFiltered" :current-page="currentPage" :per-page="perPage">
              <template slot="edit" slot-scope="row">
                <b-btn variant="warning" size="sm" @click="openEditCategoryModal(row.item)" class="mr-2">
                  Edit
                </b-btn>
              </template>
              <template slot="delete" slot-scope="row">
                <b-btn variant="danger" size="sm" @click.stop="openDeleteCategoryModal(row.item, $event.target)" class="mr-2">
                  Delete
                </b-btn>
              </template>
            </b-table>
            <b-row>
              <b-col md="6" class="my-1">
                <b-pagination :total-rows="totalRows" :per-page="perPage" v-model="currentPage" class="my-0" />
              </b-col>
            </b-row>

            <b-modal ref="editCategoryModal" size="lg" ok-variant="warning" @ok="editCategoryModalBtn()" ok-title="Edit" :title="editCategoryModal.name" :ok-disabled="$v.editCategoryModal.$invalid">
              <template>
                <b-form-group class="col-10 offset-1" label="Name:">
                  <b-form-input type="text" :class="[$v.editCategoryModal.name.$invalid ? 'is-invalid' : '']" v-model="editCategoryModal.name" placeholder="Enter name" />
                  <div class="invalid-feedback" v-if="!$v.editCategoryModal.name.required">Field is required.</div>
                  <div class="invalid-feedback" v-if="!$v.editCategoryModal.name.minLength">Field must have at least {{ $v.editCategoryModal.name.$params.minLength.min }} letters.</div>
                  <div class="invalid-feedback" v-if="!$v.editCategoryModal.name.maxLength">Field must have less than {{ $v.editCategoryModal.name.$params.maxLength.max }} letters.</div>
                </b-form-group>
              </template>
            </b-modal>

            <b-modal ref="deleteCategoryModal" size="lg" @ok="deleteCategoryModalBtn()" ok-variant="danger" ok-title="Delete" :title="deleteCategoryModal.name">
              <template>
                <h6>Are you sure you want to remove this category?</h6>
              </template>
            </b-modal>
          </template>
        </div>
      </b-tab>
      <b-tab title="Add category">
        <AddCategory />
      </b-tab>
    </b-tabs>

  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex'
import { required, minLength, maxLength } from 'vuelidate/lib/validators'
import AddCategory from './AddCategory.vue'

export default {
  components: { AddCategory },
  data() {
    return {
      fields: {
        name: {
          label: 'Name',
          sortable: true
        },
        edit: {
          label: 'Edit'
        },
        delete: {
          label: 'Delete'
        }
      },
      currentPage: 1,
      perPage: 5,
      filter: null,
      totalRows: mapState('employee', { categories: 'categories' }).length,
      pageOptions: [5, 10, 25],
      editCategoryModal: {},
      deleteCategoryModal: {}
    }
  },
  validations: {
    editCategoryModal: {
      name: {
        required,
        minLength: minLength(5),
        maxLength: maxLength(20)
      }
    }
  },
  computed: {
    ...mapState('employee', { categories: 'categories' })
  },
  methods: {
    ...mapActions('employee', [
      'getCategories',
      'updateCategory',
      'deleteCategory'
    ]),
    openEditCategoryModal(item) {
      this.editCategoryModal = JSON.parse(JSON.stringify(item))
      this.$refs.editCategoryModal.show()
    },
    editCategoryModalBtn() {
      this.updateCategory(this.editCategoryModal).then(() =>
        this.getCategories()
      )
    },
    openDeleteCategoryModal(item) {
      this.deleteCategoryModal = item
      this.$refs.deleteCategoryModal.show()
    },
    deleteCategoryModalBtn() {
      this.deleteCategory(this.deleteCategoryModal.categoryId).then(() =>
        this.getCategories()
      )
    },
    onFiltered(filteredItems) {
      this.totalRows = filteredItems.length
      this.currentPage = 1
    }
  },
  created() {
    this.getCategories()
  }
}
</script>

<style>
</style>
