import { defineStore } from 'pinia'
import functionService from '@app/services/core/function.service'

export const useFunctionStore = defineStore('useFunctionStore', {
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
            await functionService
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
            await functionService.delete(data.id)
        },

        async getByKey(key: any) {
            this.loading = true
            await functionService
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
            await functionService
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
