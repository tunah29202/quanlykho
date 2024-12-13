import { COOKIE_KEY } from '@/commons/const'
import type { AuthRequestLogin } from '@/interfaces/auth.interface'
import authService from '@app/services/core/auth.service'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('useAuthStore', {
    state: () => ({
        loggedIn: false,
        account: <any>{},
        permissions: <string[]>[],
    }),

    getters: {
        getAuth: (state) => state.account,
    },

    actions: {
        async login(data: AuthRequestLogin): Promise<boolean> {
            const response = await authService.signIn(data)
            if (response?.access_token) {
                authService.updateLocalStorage(response)
                this.loggedIn = true
                const accountInfo = await authService.getInfo()
                this.account = accountInfo
                this.permissions = accountInfo.permissions || []
                localStorage.setItem('role_cd', this.account.role_cd)
            }
            console.log(this.permissions)
            return this.loggedIn
        },
        async refresh() {
            const token = localStorage.getItem('auth.access_token')
            if (!token) return this.loggedIn
            const accountInfo = await authService.getInfo();
            if (accountInfo) {
                this.account = accountInfo;
                this.permissions = accountInfo.permissions || [];
                this.loggedIn = true;
            }
            console.log(this.permissions)
            return this.loggedIn
        },
        logout() {
            authService.clearLocalStorage()
            localStorage.removeItem('warehouse_selected')
            localStorage.removeItem('role_cd')
            this.account = {}
            this.permissions = []
            this.loggedIn = false
        },
        async setUserInfo(account: any) {
            this.account = account
            this.loggedIn = account ? true : false
        },
        checkPermission(permission: string): boolean {
            return this.permissions.includes(permission)
        },
    },
})