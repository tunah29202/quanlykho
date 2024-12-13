<template>
    <vc-modal ref="modal" :title="tl('Invoice', 'title_modal_order_add')" width="70%" @close="close">
        <template #content>
            <vc-row :gutter="12" class="mt-4" style="padding: 12px 16px 0 ">
                <vc-col :span="8">
                    <el-input v-model="search" :placeholder="tl('Common', 'input_search_holder', [tl('Order', 'order_text')])" :prefix-icon="Search" @keyup.enter="onSearch" />
                </vc-col>
                <vc-col :span="8">
                    <el-button type="primary" @click="onSearch" class="">
                        {{ tl("Common", "btn_search") }}
                    </el-button>
                </vc-col>
            </vc-row>
            <vc-row style="padding: 12px; background-color: #ececec; margin: 12px 16px">
                <vc-col>
                    <vc-table :datas="dataGrid" :tableConfig="tableConfig" :colConfigs="colConfig" :page="pageConfig"
                    @pageChanged="onPageChanged" :loading="loading">
                    <template #action="{data}">
                        <div class="d-flex flex-center">
                            <vc-button type="primary" code="F00015" size="small" class="btn-acttion btn-action-select" @click="onSelect(data)">
                            Chọn
                            </vc-button>
                        </div>
                    </template>
                    </vc-table>
                </vc-col>
            </vc-row>
        </template>
        <vc-confirm ref="confirmDialog"></vc-confirm>
    </vc-modal>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import { useOrderStore } from '@app/stores/app/order.store';
    import { storeToRefs } from 'pinia';
    import { colConfig } from '@/commons/config/app/order.config'
    import { Search } from '@element-plus/icons-vue';

    const storeOrder = useOrderStore();
    const {dataGrid, pageConfig, search, loading} = storeToRefs(storeOrder)
    const confirmDialog = ref<any>(null);

    const modal = ref<any>(null);
    const emit = defineEmits(['setOrder'])


    const onPageChanged = async (page: any) => {
        pageConfig.value = { ...page };
        onSearch()
    };

    const onSearch = async () => {
        await storeOrder.getNotInInvoice()
    }
    
    const tableConfig = {
        checkbox: false,
        action: true,
        showPaging: true,
        dbClick: false,
        index: true,
        order:true,
    }
    const onSelect = async (item: any) =>{
        emit('setOrder', item)
        close()
    }
    const open = async () => {
        await storeOrder.getNotInInvoice();
        modal.value.open();
    };
    const close = () => {
        modal.value.close()
    };

    defineExpose({
        open,
        close,
    });


</script>
<style>
    .el-form-item {
    display: block;
    }

    .btn-group-action .btn-action-select {
    width: auto !important;
    padding: 4px !important;
    }
</style>