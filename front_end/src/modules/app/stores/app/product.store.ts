import { defineStore } from 'pinia'
import productService from '@app/services/app/product.service'
import number from '@/utils/number'

export const useProductStore = defineStore('useProductStore', {
    state: () => ({
        dataGrid: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
        status: true,
        catogory_name: <any>[],
        warehouse_id: <any>[],
        pageConfig: <any>[],
        loading: false,
    }),
    getters: {
        getData(state) {
            return state.dataGrid
        },
    },
    actions: {
        async getList() {
            this.loading = true
            await productService
                .getList({
                    sort: this.goSort,
                    is_actived: true,
                    search: this.search,
                    status: this.status,
                    warehouse_id : this.warehouse_id,
                    is_exists_in_warehouse: false,
                    ...this.pageConfig,
                })
                .then((data) => {
                    this.dataGrid = data.data ?? []
                    this.pageConfig.total = data.total
                })
                .finally(() => {
                    this.loading = false
                })
        },

        async getProductInventory() {
            this.loading = true
            await productService
                .getProductInventory({
                    sort: this.goSort,
                    is_actived: true,
                    search: this.search && this.search.toString(),
                    status: this.status,
                    category_name: this.catogory_name && this.catogory_name.toString(),
                    warehouse_id : this.warehouse_id,
                    is_exists_in_warehouse: false,
                    product_ids_in_carton: null,
                    ...this.pageConfig,
                })
                .then((data) => {
                    this.dataGrid = data.data ?? []
                    this.dataGrid = this.dataGrid.map((item: any) => {
                        if(item.price_unit)
                        {
                            const formated = number.formatCurrency(item.price_unit)
                            item.price_unit = formated;
                        };
                        return;
                    })
                    this.pageConfig.total = data.total
                })
                .finally(() => {
                    this.loading = false
                })
        },

        async delete(data: any) {
            await productService.delete(data.id)
        },
        
        setWarehouseId(item: any){
            this.warehouse_id = item;
        },

        async getByKey(key: any) {
            this.loading = true
            await productService
                .detail(key)
                .then((data) => {
                    this.formData = data.data ?? {}
                })
                .finally(() => {
                    this.loading = false
                })
        },

        async export() {
            this.loading = true
            await productService
                .export({
                    sort: this.goSort,
                    is_actived: true,
                    search: this.search,
                    ...this.pageConfig,
                    size: 1000000,
                })
                .finally(() => {
                    this.loading = false
                })
        },
    },
})
