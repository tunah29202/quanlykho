import { defineStore } from 'pinia'
import productService from '@app/services/app/product.service'
import number from '@/utils/number'

export const useProductStore = defineStore('useProductStore', {
    state: () => ({
        dataGrid: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
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
                    search: this.search,
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

        async delete(data: any) {
            await productService.delete(data.id)
        },
        
        setWarehouseId(item: any){
            this.warehouse_id = item;
        },

        async getProductInventory(array: any){
            this.loading = true;
            await productService.getProductInventory({
                search: this.search && this.search.toString(),
                warehouse_id: this.warehouse_id,
                product_ids_in_carton: array,
                ...this.pageConfig
            })
            .then((data)=>{
                this.dataGrid = data.data ??[]
                this.dataGrid = this.dataGrid.map((item: any) => {
                    if(item.price_unit)
                    {
                        const formated = number.formatCurrency(item.price_unit)
                        item.price_unit = formated;
                    };
                    return item;
                })
                this.pageConfig.total = data.total
            })
            .finally(()=>{
                this.loading = false
            })
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
