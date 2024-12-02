<template>
    <div class="vc-page page-role">
        <vc-card>
            <h1 class="pb-4" style="border-bottom: 1px solid #cdcdcd">
                {{ tl("Common", "manage_text", [tl("Role", "role_text")]) }}
            </h1>
            <vc-row :gutter="12" class="mt-4">
                <vc-col :span="8">
                    <el-input v-model="search" :placeholder="tl('Common', 'input_search_holder', [tl('Role', 'role_text')])" :prefix-icon="Search" @keyup.enter="onSearch" />
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
                    <vc-col :span="6" class="d-flex justify-content-end">
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
    </div>
</template>
<script setup lang="ts">
    import { storeToRefs } from 'pinia'
    import { onMounted, ref } from 'vue'
    import tl from '@/utils/locallize'
    import { colConfig, tableConfig } from '@/commons/config/core/role.config'
    import { useRoleStore } from '@app/stores/core/role.store'
    import { POPUP_TYPE } from '@/commons/const'
    import { Search } from '@element-plus/icons-vue'
    import DetailModal from './DetailModal.vue'

    const store = useRoleStore();
    const { dataGrid, pageConfig, search, loading } = storeToRefs(store);
    const popupType = ref<POPUP_TYPE>(POPUP_TYPE.CREATE);
    const confirmDialog = ref<any>(null);
    const detailRef = ref<any>(null);

    onMounted(async () => {
        await onSearch()
    });

    const onSearch = async () => {
        await store.getList()
    }

    const onPageChanged = async (page: any) => {
        pageConfig.value = { ...page };
        onSearch()
    };
    const onAddNew = () => {
        popupType.value = POPUP_TYPE.CREATE
        detailRef.value.open(tl("Common", "title_modal_add", [tl("Role", "role_text")]), null, async (res: any) => {
            if (res) await onSearch()
        })
    };

    const onEdit = (item: any) => {
        popupType.value = POPUP_TYPE.EDIT;
        detailRef.value.open(tl("Common", "title_modal_edit", [tl("Role", "role_text")]), item.id, async (res: any) => {
            if (res) await onSearch()
        })
    };

    const onView = (item: any) => {
        popupType.value = POPUP_TYPE.VIEW;
        detailRef.value.open(tl("Common", "title_modal_detail", [tl("Role", "role_text")]), item.id)
    };

    const onDeleteItem = (item: any) => {
        confirmDialog.value.confirm(
            tl("Common", "title_modal_delete"),
            tl("Common", "comfirm_delete", [tl("Role", "role_text")]),
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