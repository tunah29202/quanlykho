import { defineStore } from 'pinia'
import shipperService from '@app/services/app/shipper.service'

export const useShipperStore = defineStore('useShipperStore', {
    state: () => ({
        dataGrid: <any>[],
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
            await shipperService
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

        async delete(data: any) {
            await shipperService.delete(data.id)
        },

        async getByKey(key: any) {
            this.loading = true
            await shipperService
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
            await shipperService
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
