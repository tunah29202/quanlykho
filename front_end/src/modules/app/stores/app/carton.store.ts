import { defineStore } from 'pinia'
import cartonService from '@app/services/app/carton.service'

export const useCartonStore = defineStore('useCartonStore', {
    state: () => ({
        dataGrid: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
        category_name: <any>[],
        pageConfig: <any>[],
        warehouse_id: <any>[],
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
            await cartonService
                .getList({
                    sort: this.goSort,
                    search: this.search,
                    warehouse_id : this.warehouse_id,
                    category_name: this.category_name,
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
        async getNotInInvoice() {
            this.loading = true
            await cartonService
                .getNotInInvoice({
                    sort: this.goSort,
                    search: this.search,
                    warehouse_id : this.warehouse_id,
                    category_name: this.category_name,
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
            await cartonService.delete(data.id)
        },
        setWarehouseId(item: any){
            this.warehouse_id = item;
        },
        async getByKey(key: any) {
            this.loading = true
            await cartonService
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
            await cartonService
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
