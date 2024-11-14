import { defineStore } from 'pinia'
import tl from '@/utils/locallize'
import apiClient from '@/utils/httpClient'
import { useToast } from '@/components/commons/alert/vc-toast.vue'
import fileService from '@/utils/file'
import { ResponseCode } from '@/commons/const'

export const useImportStore = defineStore('useImportStore', {
    state: () => ({
        files: [] as File[],
        isError: false,
        previousUploadFileName: '',
        dataValiable: {
            fileName: '',
            fileList: [] as File[], 
            haveError: false,
            file: null as File | null,
        },
        constEnum: {
            extensionsFileUpload: ['.xls', '.xlsx'],
            maxSize: 5, // MB
        },
    }),

    actions: {
        open(importDialog: any) {
            importDialog.open()
        },

        close(importDialog: any, uploadRef: any) {
            this.clearFiles(uploadRef)
            importDialog.close()
        },

        clearFiles(uploadRef: any) {
            this.dataValiable = { fileName: '', haveError: false, file: null, fileList: [] } 
            this.isError = false
            uploadRef?.clearFiles()
        },

        async onDownloadTemplate(template: { exportUrl: string; fileName: string }) {
            try {
                const response = await apiClient.get(template.exportUrl, { responseType: 'blob' })
                fileService.resolveAndDownloadBlob(response, template.fileName)
            } catch {
                useToast.push({ message: tl('Common', 'download_error'), type: 'error' })
            }
        },

        async onImport(url: string, params: { isError: boolean }, onSuccess: () => void, warehouseId: string | number) {
            if (this.dataValiable.haveError || !this.dataValiable.file) {
                useToast.push({ message: tl('Common', 'E_FileRequired'), type: 'error' })
                return
            }

            const formData = new FormData()
            formData.append('file', this.dataValiable.file)
            formData.append('warehouse_id', String(warehouseId))

            try {
                const response = await apiClient.post(url, formData, { params })
                this.handleResponse(response, params, onSuccess)
            } catch {
                toast.push({ message: tl('Common', 'import_failed'), type: 'error' })
            }
        },

        handleResponse(response: any, params: { isError: boolean }, onSuccess: () => void) {
            if (response.code === ResponseCode.Success) {
                useToast.handleApiResponse(response);
                onSuccess()
                return
            }

            this.isError = response.code === ResponseCode.SystemError
            useToast.push({ message: response.message, type: 'error' })
            this.dataValiable.haveError = true

            if (params.isError) {
                fileService.resolveAndDownloadBlob(response, 'insert_products_error.xlsx')
            }
        },

        fileHandleChange(uploadFiles: File[]) {
            const file = uploadFiles[0]
            this.dataValiable.fileName = file?.name || ''
            this.previousUploadFileName = file?.name || ''
            this.dataValiable.file = file || null;
            this.dataValiable.fileList = uploadFiles; 
            this.dataValiable.haveError = this.validateFile(file)
        },

        fileHandleRemove(file: File) {
            this.dataValiable.fileList = this.dataValiable.fileList.filter(f => f !== file);
            if (this.dataValiable.file === file) {
                this.dataValiable.file = null;
            }
        },

        validateFile(file: File) {
            if (!file) return false
            const sizeValid = file.size <= this.constEnum.maxSize * 1024 * 1024
            const extValid = this.constEnum.extensionsFileUpload.includes(`.${file.name.split('.').pop()?.toLowerCase()}`)
            return !(sizeValid && extValid)
        },
    },
})
