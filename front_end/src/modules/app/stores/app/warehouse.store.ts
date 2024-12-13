import { defineStore } from 'pinia'
import warehouseService from '@app/services/app/warehouse.service'

export const useWarehouseStore = defineStore('useWarehouseStore', {
    state: () => ({
        dataGrid: <any>[],
        dataGridAll: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
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
            await warehouseService
                .getList({
                    sort: this.goSort,
                    search: this.search,
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
        async getAll() {
            this.loading = true
            await warehouseService
                .getList({
                    sort: this.goSort,
                    search: this.search,
                    get_all: true,
                    ...this.pageConfig,
                })
                .then((data) => {
                    this.dataGridAll = data.data ?? []
                    this.pageConfig.total = data.total
                })
                .finally(() => {
                    this.loading = false
                })
        },

        async delete(data: any) {
            await warehouseService.delete(data.id)
        },

        async getByKey(key: any) {
            this.loading = true
            await warehouseService
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
            await warehouseService
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
        setWarehouseSelected(data: any){
            localStorage.setItem('warehouse_selected', JSON.stringify(data));
        },
    },
})
