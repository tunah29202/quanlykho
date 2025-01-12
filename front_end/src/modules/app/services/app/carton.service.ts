import apiClient from '@/utils/httpClient'
import { API } from '@/commons/config/app/carton.config'
import type { APIResponse } from '@/interfaces/response.interface'
import  useToast from '@/components/commons/alert/vc-toast.vue'
import fileService from '@/utils/file'

const cartonService = {
    async getList(params?: unknown): Promise<APIResponse<any[]>> {
        const res = await apiClient.get(API.LIST, {
            params: params,
        })
        return res;
    },
    async getNotInInvoice(params?: unknown): Promise<APIResponse<any[]>> {
        const res = await apiClient.get(API.GET_NOT_IN_INVOICE, {
            params: params,
        })
        return res;
    },
    async detail(id: string): Promise<APIResponse<any>> {
        const res = await apiClient.get(API.DETAIL(id));
        return res.data;    
    },

    async export(params?: unknown) {
        return await apiClient
            .get(API.EXPORT, {
                params: params,
                responseType: 'blob',
            })
            .then((response: any) => {
                fileService.resolveAndDownloadBlob(response, `Danhsachguoidung.xlsx`);
            })
    },

    async create(data: any) {
        return await apiClient
            .post(API.CREATE, data)
            .then((response: any) => {
                useToast.handleApiResponse(response);
                return response
            })
    },

    async update(data: any) {
        return await apiClient
            .put(API.UPDATE(data.id || ''), data)
            .then((response: any) => {
                useToast.handleApiResponse(response);
                return response
            })
    },

    async delete(id: string) {
        return await apiClient
            .delete(API.DELETE(id))
            .then((response: any) => {
                useToast.handleApiResponse(response);
                return response
            })
    },

    async deleteMulti(ids: string[]) {
        return await apiClient
            .delete(API.DELETE_MULTI, { data: ids })
            .then((response: any) => {
                useToast.handleApiResponse(response);
                return response
            })
    },

}

export default cartonService
