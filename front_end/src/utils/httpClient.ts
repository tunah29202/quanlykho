import axios from 'axios'
import type {
    AxiosRequestHeaders,
    AxiosError,
    AxiosInstance,
    AxiosRequestConfig,
    AxiosResponse,
} from 'axios'
import { useToast } from '@/components/commons/alert/vc-toast.vue'
import type { APIResponse } from '@/interfaces/response.interface'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    headers: {
        'Content-Type': 'multipart/form-data',
    },
})

const onRequest = (config: AxiosRequestConfig): AxiosRequestConfig => {
    const token = localStorage.getItem('auth.access_token')
    const header = <AxiosRequestHeaders>{}
    if (token) {
        header.Authorization = `Bearer ${token}`
    }

    const requestConfig: AxiosRequestConfig = {
        ...config,
        headers: header,
    }
    return requestConfig
}

const onRequestError = (error: AxiosError): Promise<AxiosError> => {
    return Promise.reject(error)
}

const onResponse = (response: AxiosResponse): Promise<APIResponse<any>> => {
    const responseData: AxiosResponse = {
        ...response,
    }
    return responseData.data
}

const onResponseError = (error: AxiosError): Promise<AxiosError> => {
    useToast.handleApiResponse({
        message: error?.response?.data?.message,
        code: error?.response?.status || 500,
    })
    return Promise.resolve(error)
}

const setupInterceptorsTo = (axiosInstance: AxiosInstance): AxiosInstance => {
    axiosInstance.interceptors.request.use(onRequest, onRequestError)
    axiosInstance.interceptors.response.use(onResponse, onResponseError)
    return axiosInstance
}

setupInterceptorsTo(apiClient)

export default apiClient
