import { defineStore } from 'pinia'
import consigneeService from '@app/services/app/consignee.service'

export const useConsigneeStore = defineStore('useConsigneeStore', {
    state: () => ({
        dataGrid: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
        status: true,
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
            await consigneeService
                .getList({
                    sort: this.goSort,
                    is_actived: true,
                    search: this.search,
                    status: this.status,
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
            await consigneeService.delete(data.id)
        },

        async getByKey(key: any) {
            this.loading = true
            await consigneeService
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
            await consigneeService
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
