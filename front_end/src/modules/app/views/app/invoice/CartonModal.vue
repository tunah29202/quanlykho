<template>
    <vc-modal ref="modal" :title="tl('Invoice', 'title_modal_carton_add')" width="80%" @close="close">
        <template #content>
            <vc-row :gutter="12" class="mt-4" style="padding: 12px 16px 0 ">
                <vc-col :span="8">
                    <el-input v-model="search" :placeholder="tl('Common', 'input_search_holder', [tl('Carton', 'carton_text')])" :prefix-icon="Search" @keyup.enter="onSearch" />
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
                              :loading="loading" @pageChanged="onPageChanged">
                        <template #detail="scope">
                            <el-table :data="scope.data" border>
                                <el-table-column label="#" width="50" type="index" align="center" header-align="center"/>
                                <el-table-column :label='tl("Product", "code_text")' prop="product.code"/>
                                <el-table-column :label='tl("Category", "name_text")' prop="product.category.name"/>
                                <el-table-column :label='tl("Product", "name_text")' prop="product.name"/>
                                <el-table-column :label='tl("Product", "quantity_text")' prop="quantity"/>
                                <el-table-column :label='tl("Product", "price_unit_text")' prop="product.price_unit"/>
                                <el-table-column :label='tl("Carton", "unit_text")' prop="unit"/>
                            </el-table>
                        </template>      
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
    import { useCartonStore } from '@app/stores/app/carton.store';
    import { storeToRefs } from 'pinia';
    import { colConfig, tableConfig } from '@/commons/config/app/carton.config'
    import { Search } from '@element-plus/icons-vue';

    const storeCarton = useCartonStore();
    const {dataGrid, pageConfig, search, loading} = storeToRefs(storeCarton)
    const modal = ref<any>(null);
    const emit = defineEmits(['setCarton'])


    const onPageChanged = async (page: any) => {
        pageConfig.value = { ...page };
        onSearch()
    };

    const onSearch = async () => {
        await storeCarton.getList()
    }
    const onSelect = async (item: any) =>{
        emit('setCarton', item)
        close()
    }
    const open = async () => {
        await storeCarton.getList();
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