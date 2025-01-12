import apiClient from '@/utils/httpClient'
import { API } from '@/commons/config/app/product.config'
import type { APIResponse } from '@/interfaces/response.interface'
import useToast from '@/components/commons/alert/vc-toast.vue'
import fileService from '@/utils/file'

const productService = {
    async getList(params?: unknown): Promise<APIResponse<any[]>> {
        return await apiClient.get(API.LIST, {
            params: params,
        })
    },
    async getProductInventory(data?: any) {
        return await apiClient.post(API.GET_PRODUCT_INVENTORY, data)
        .then((response: any) => {
            return response
        })
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
                fileService.resolveAndDownloadBlob(response, `Danhsachsanpham.xlsx`);
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

    async update(product_id: any, data: any) {
        return await apiClient
            .put(API.UPDATE(product_id || ''), data)
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

export default productService
