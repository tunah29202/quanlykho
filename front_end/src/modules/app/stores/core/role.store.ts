import { defineStore } from 'pinia'
import roleService from '@app/services/core/role.service'

export const useRoleStore = defineStore('useRoleStore', {
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
            await roleService
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
            await roleService.delete(data.id)
        },

        async getByKey(key: any) {
            this.loading = true
            await roleService
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
            await roleService
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
