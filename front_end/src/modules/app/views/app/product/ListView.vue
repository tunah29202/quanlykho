<template>
    <div class="vc-page page-product">
        <vc-card>
            <h1 class="pb-4" style="border-bottom: 1px solid #cdcdcd">
                {{ tl("Common", "manage_text", [tl("Product", "product_text")]) }}
            </h1>
            <vc-row :gutter="12" class="mt-4">
                <vc-col :span="8">
                    <el-input v-model="search" :placeholder="tl('Common', 'input_search_holder', [tl('Product', 'product_text')])" :prefix-icon="Search" @keyup.enter="onSearch" />
                </vc-col>
                <vc-col :span="8">
                    <el-button type="primary" @click="onSearch" class="">
                        {{ tl("Common", "btn_search") }}</el-button>
                </vc-col>
            </vc-row>
        </vc-card>
        <vc-card style="margin-top: 20px;">
            <div class="flex-end mt-auto">
                <vc-row class="mt-4 mb-4">
                    <vc-col :span="6" style="display: flex;">
                        <vc-button class="pr-5" @click="onExport" type="default" :icon="'Download'">
                            {{ tl("Common", "BtnExportExcel") }}
                        </vc-button>
                        <vc-button class="pr-5" @click="onImport" type="default" :icon="'Upload'">
                            {{ tl("Common", "BtnImportExcel") }}
                        </vc-button>
                        <vc-button class="pr-5" @click="onAddNew" type="primary" :icon="'Plus'">
                            {{ tl("Common", "btn_add_new") }}
                        </vc-button>
                    </vc-col>
                </vc-row>
            </div>

            <vc-row>
                <vc-col :span="24">
                    <vc-table :datas="dataGrid" :tableConfig="tableConfig" :colConfigs="colConfig" :page="pageConfig"
                              :loading="loading" @pageChanged="onPageChanged">
                        <template #action="{data}">
                            <div class="d-flex flex-center">
                                <vc-button type="warning" size="small" class="btn-acttion" @click="onView(data)" :icon="'View'">
                                </vc-button>
                                <vc-button type="primary" size="small" class="btn-acttion" @click="onEdit(data)" :icon="'Edit'">
                                </vc-button>
                                <vc-button type="danger" code="F00015" size="small" class="btn-acttion" @click="onDeleteItem(data)" :icon="'Delete'">
                                </vc-button>
                            </div>
                        </template>
                    </vc-table>
                </vc-col>
            </vc-row>
        </vc-card>
        <vc-confirm ref="confirmDialog"></vc-confirm>
        <detail-modal ref="detailRef" :type="popupType"></detail-modal>
        <vc-import ref="importDialogPrd" :urlImport="urlImport" :template="template" :onSuccess="onSuccess" :warehouse_id="warehouse_id"></vc-import>
    </div>
</template>
<script setup lang="ts">
    import { storeToRefs } from 'pinia'
    import { onMounted, ref } from 'vue'
    import tl from '@/utils/locallize'
    import { colConfig, tableConfig } from '@/commons/config/app/product.config'
    import { useProductStore } from '@app/stores/app/product.store'
    import { POPUP_TYPE } from '@/commons/const'
    import { Download, Search, Upload } from '@element-plus/icons-vue'
    import DetailModal from './DetailModal.vue'
    import VcImport from '@/components/commons/import/vc-import.vue'

    const store = useProductStore();
    const { dataGrid, pageConfig, search, loading, warehouse_id } = storeToRefs(store);
    const popupType = ref<POPUP_TYPE>(POPUP_TYPE.CREATE);
    const confirmDialog = ref<any>(null);
    const importDialogPrd = ref<any>(null);
    const detailRef = ref<any>(null);

    const urlImport = ref("product/import-product")
    const template = ref({
    exportUrl: "product/download-file-template",
    fileName: "TEMPLATE_IMPORT_PRODUCT.xlsx",
    })

    onMounted(async () => {
        await onSearch()
    });

    const onSearch = async () => {
        await store.getList()
    }
    const onSuccess = async () => {
        importDialogPrd.value.close()
        await store.getList();
    }
    const onPageChanged = async (page: any) => {
        pageConfig.value = { ...page };
        onSearch()
    };

    const onImport = ()=>{
        importDialogPrd.value.open()
    }

    const onExport = () => {
        store.export()
    };

    const onAddNew = () => {
        popupType.value = POPUP_TYPE.CREATE
        detailRef.value.open(tl("Common", "title_modal_add", [tl("Product", "product_text")]), null, async (res: any) => {
            if (res) await onSearch()
        })
    };

    const onEdit = (item: any) => {
        popupType.value = POPUP_TYPE.EDIT;
        detailRef.value.open(tl("Common", "title_modal_edit", [tl("Product", "product_text")]), item.id, async (res: any) => {
            if (res) await onSearch()
        })
    };

    const onView = (item: any) => {
        popupType.value = POPUP_TYPE.VIEW;
        detailRef.value.open(tl("Common", "title_modal_detail", [tl("Product", "product_text")]), item.id)
    };

    const onDeleteItem = (item: any) => {
        confirmDialog.value.confirm(
            tl("Common", "title_modal_delete"),
            tl("Common", "comfirm_delete", [tl("Product", "product_text")]),
            async (res: any) => {
                if (res) {
                    await store.delete(item);
                    await onSearch()
                }
            }
        );
    };
</script>
<style >
    
</style>