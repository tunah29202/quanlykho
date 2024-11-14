import { COOKIE_KEY } from '@/commons/const'
import type { AuthRequestLogin } from '@/interfaces/auth.interface'
import authService from '@app/services/core/auth.service'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('useAuthStore', {
    state: () => ({
        loggedIn: false,
        account: <any>{},
    }),

    getters: {
        getAuth: (state) => state.account,
    },

    actions: {
        async login(data: AuthRequestLogin): Promise<boolean> {
            console.log('response')
            const response = await authService.signIn(data)
            if (response?.access_token) {
                authService.updateLocalStorage(response)
                this.loggedIn = true
                this.account = (await authService.getInfo())
            }
            return this.loggedIn
        },
        async refresh() {
            const token = localStorage.getItem('auth.access_token')
            if (!token) return this.loggedIn
            this.account = (await authService.getInfo())
            if (this.account) this.loggedIn = true
            return this.loggedIn
        },
        logout() {
            authService.clearLocalStorage()
            localStorage.removeItem('warehouse_selected')
            localStorage.removeItem('role_cd')
            this.account = {}
            this.loggedIn = false
        },
        async setUserInfo(account: any) {
            this.account = account
            this.loggedIn = account ? true : false
        },
    },
})