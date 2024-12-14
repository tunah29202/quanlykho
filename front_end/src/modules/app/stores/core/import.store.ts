import { defineStore } from 'pinia'
import tl from '@/utils/locallize'
import apiClient from '@/utils/httpClient'
import useToast from '@/components/commons/alert/vc-toast.vue'
import fileService from '@/utils/file'
import { ResponseCode } from '@/commons/const'
import datetime from '@/utils/datetime'


export const useImportStore = defineStore('useImportStore', {
    state: () => ({
      files: <File[]>[],
      mode: null,
      isError: <any>false,
      previousUploadFiles: <any>null,
      error: <any[]>[],
      importDialog: <any>null,
      uploadRef: <any>null,
      dataValiable: {
        fileName: <string>'',
        messages: <string>'',
        haveError: false,
        fileList: <UploadUserFile[]>[],
        uploadRef: <any>{},
        file: <any>null,
      },
      constEnum: {
        extensionsFileUpload: ['.xls', '.xlsx'],
        maxSize: 5, // đơn vị MB
      },
    }),
    getters: {
      getItems(state) {
        return state.files
      },
    },
    actions: {
      open() {
        this.importDialog.open()
      },
      close() {
        this.clearFiles()
        this.importDialog.close()
      },
      clearFiles() {
        this.dataValiable.fileName = ''
        this.dataValiable.file = null
        this.dataValiable.messages = ''
        this.dataValiable.haveError = false
        this.isError = false
        this.uploadRef.clearFiles()
      },
      onDownloadTemplate(template: any) {
        apiClient
          .get(template.exportUrl, {
            responseType: 'blob',
          })
          .then((response: any) => {
            if (!response.code && !response.message) {
              fileService.resolveAndDownloadBlob(response, template.fileName)
            }
          })
      },
      onImport(url: string, params: any, onSuccess: any, warehouse_id: any) {
        const configResponse = params.isError ? { responseType: 'blob' } as any : {}
  
        if (!this.dataValiable.haveError && this.dataValiable.file) {
          const _file: any = this.dataValiable.file
          const fromData = new FormData()
          fromData.append('file', _file.raw)
          fromData.append('warehouse_id', warehouse_id)
          apiClient.post(url, fromData, { params: params, ...configResponse }).then((response: any) => {
            if (response.code != undefined) {
              if (response.code == ResponseCode.SystemError) {
                this.isError = true;
              }
              if (response.code != ResponseCode.Success) {
                if (!params.isError) {
                  useToast.push({
                    message: response.message,
                    type: 'error',
                  });
                }
                this.dataValiable.messages = response.message;
              }
              else {
                // Xử lý khi không có lỗi
                useToast.handleApiResponse(response);
                onSuccess();
                this.close();
              }
            } else {
              // Xử lý khi response.code không tồn tại
              if (params.isError == false) {
                params.isError = true;
                this.onImport(url, params, onSuccess, warehouse_id);
                return;
              }
            }
  
            // Xử lý khi có lỗi
            if (params.isError) {
              useToast.push({
                message: tl('Common', 'import_error', [`insert_products_error_${datetime.formatDateTimeNew(new Date().toDateString())}.xlsx`]),
                type: 'error',
              });
              fileService.resolveAndDownloadBlob(response, 'insert_products_error.xlsx');
              params.isError = false;
            }
  
          })
        } else {
          useToast.push({
            message: tl('Common', 'E_FileRequired'),
            type: 'error',
          })
        }
      },
      fileHandleChange(uploadFile: any, uploadFiles: any) {
        const isChanged = uploadFiles[0].name !== this.previousUploadFiles;
        this.previousUploadFiles = uploadFiles[0].name;
        if (isChanged) {
          this.isError = false;
        }
  
        this.generateMessage(uploadFile, uploadFiles)
      },
      fileHandleRemove(uploadFile: any, uploadFiles: any) {
        this.generateMessage(uploadFile, uploadFiles)
      },
      generateMessage(uploadFile: any, uploadFiles: any) {
        if (!uploadFiles.length) {
          this.dataValiable.fileName = ''
          this.dataValiable.file = null
          this.dataValiable.messages = ''
          return
        }
        this.dataValiable.haveError = false
        this.uploadRef.clearFiles()
        this.dataValiable.fileName = uploadFiles[0].name
        const _errors = []
        if (uploadFiles[0].size > this.constEnum.maxSize * 1024 * 1024) {
          this.dataValiable.haveError = true
          _errors.push({
            message: tl('FileUpload', 'E_FileSizeLength', [
              this.constEnum.maxSize + 'MB',
            ]),
          })
        }
  
        // Error Extension
        const extensionFileCurrent =
          '.' + this.dataValiable.fileName.split('.').pop()
        if (
          this.constEnum.extensionsFileUpload.indexOf(extensionFileCurrent) == -1
        ) {
          this.dataValiable.haveError = true
          _errors.push({
            message: tl('FileUpload', 'E_Extension', [
              this.constEnum.extensionsFileUpload.join(', '),
            ]),
          })
        }
        this.dataValiable.messages = _errors
          .map((x: any) => x.message)
          .join('<br>')
        this.dataValiable.file = uploadFiles[0]
      },
    },
  })
  