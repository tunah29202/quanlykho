import apiClient from '@/utils/httpClient'
import type {
    AuthRequestLogin,
    AuthResponse,
    AuthChangePassword,
} from '@/interfaces/auth.interface'
import type { APIResponse } from '@/interfaces/response.interface'
import { API } from '@/commons/config/core/auth.config'
import { COOKIE_KEY } from '@/commons/const'
import { useToast } from '@/components/commons/alert/vc-toast.vue'

const authService = {
    async signIn(data: AuthRequestLogin): Promise<APIResponse<AuthResponse>> {
        return await apiClient.post(API.SIGN_IN, data)
    },

    async getInfo(): Promise<APIResponse<any>> {
        return await apiClient.get(API.GET_INFO)
    },
    async clearLocalStorage() {
        localStorage.removeItem(COOKIE_KEY.ACCESS_TOKEN)
        localStorage.removeItem(COOKIE_KEY.REFRESH_TOKEN)
    },
    async updateLocalStorage(data: AuthResponse) {
        localStorage.setItem(COOKIE_KEY.ACCESS_TOKEN, data.access_token)
        localStorage.setItem(COOKIE_KEY.REFRESH_TOKEN, data.refresh_token)
    },
    async updateCookies(data: AuthResponse) {
        document.cookie = `${COOKIE_KEY.ACCESS_TOKEN}=${data.access_token}; ${COOKIE_KEY.REFRESH_TOKEN}=${data.refresh_token}`;
    },
    async clearCookies() {
        document.cookie = '';
    },
    async changePassword(data: AuthChangePassword) {
        return await apiClient
            .put(API.CHANGE_PASSWORD, data)
            .then((response: any) => {
                useToast.handleApiResponse(response);
                return response
            })
    },
    async sendEmail(data: any): Promise<APIResponse<any[]>> {
        return await apiClient.post(API.SEND_EMAIL, data)
    },
    async checkCodePass(data: any): Promise<APIResponse<any[]>> {
        return await apiClient.post(API.CHECK_CODE_PASS, data)
    },
    async forgotPassword(data: any): Promise<APIResponse<any[]>> {
        return await apiClient.put(API.FORGOT_PASSWORD, data)
    },
}

export default authService
