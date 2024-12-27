<template>
    <vc-modal ref="modalWarehouse" :title="tl('User', 'title_modal_warehouse_add')" @close="close" >
        <template #content >
            <vc-row style="padding: 12px 16px 0;" >
                <vc-col :span="14" style="display: flex;" >
                    <el-input v-model="search" :placeholder="tl('Warehouse', 'input_search_holder')" :prefix-icon="Search" />
                    <el-button type="primary" @click="onSearch" class="ml-2">{{ tl("Common", "btn_search") }}</el-button>
                </vc-col>
            </vc-row>
            <el-form ref="warehouseForm" :model="warehouse" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right">
                <vc-row :gutter="20" class="mt-3">
                    <vc-col>
                        <el-card shadow="always" style="padding: 0;" >
                            <vc-treeview show-checkbox node_key="id" v-model="warehouse.warehouse_ids" :data="dataGrid" />
                            <div>
                                <vc-pagination :pageConfig="pageConfig" @changed="onPageChanged" ></vc-pagination>
                            </div>
                        </el-card>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button type="primary" class="ml-2" code="F00004" @click="onSave(warehouseForm)" :loading="isLoading"
            :icon="'FolderChecked'">
            {{ tl("Common", "btn_add") }}
            </vc-button>
        </template>
        <vc-confirm ref="confirmDialog" ></vc-confirm>
    </vc-modal>
</template>
<script setup lang="ts" >
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import userService from "@app/services/core/user.service";
    import { useRoleStore } from '@app/stores/core/role.store';
    import { storeToRefs } from 'pinia';
    import { FolderChecked, Search } from '@element-plus/icons-vue';
    import { useWarehouseStore } from '@app/stores/app/warehouse.store';
    import type { FormInstance } from 'element-plus';
    import VcPagination from '@/components/commons/table/vc-pagination.vue';
    
    const storeWarehouse = useWarehouseStore();
    const {dataGrid, search, pageConfig, loading} = storeToRefs(storeWarehouse);
    const modalWarehouse = ref<any>(null);
    const warehouseForm = ref<any>(null);
    const confirmDialog = ref<any>(null);
    const isLoading = ref(false);
    const emit = defineEmits('setWarehouse_ids', 'setWarehouse_names')
    
    const open = async (item: any) => {
        if (item)
        warehouse.id = item
        modalWarehouse.value.open();
        await getWarehouse();
    };
    const close = () => {
        warehouse.warehouse_ids = []
        warehouse.warehouse_names = []
        modalWarehouse.value.close()
    };
    
    const warehouse = reactive({
        id:'',
        warehouse_ids:[],
        warehouse_names:[]
    });
    onBeforeMount(async ()=>{
        await init()
        
    });
    const onSearch = async ()=>{
        await storeWarehouse.getList()
    }
    const getWarehouse = async ()=>{
        const res = await userService.detail(warehouse.id);
        Object.assign(warehouse, res);
    }
    const init = async ()=>{
        await storeWarehouse.getList();
    }
    const onSave = async (formEl: FormInstance | undefined)=>{
        if(!formEl) return;
        await formEl.validate(async (valid)=>{
            if(!valid) return;
            isLoading.value = true;
            warehouse.warehouse_ids = warehouse.warehouse_ids ?? [];
            warehouse.warehouse_names = dataGrid.value.filter((data: any)=>warehouse.warehouse_ids.includes((data.id))).map((data: any)=>data.name);
            emit('setWarehouse_ids', warehouse.warehouse_ids);
            emit('setWarehouse_names', warehouse.warehouse_names)
            isLoading.value=false;
            close();
        })
    }
    
    const onPageChanged = async (page:any)=>{
        pageConfig.value = {...page};
        onSearch()
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
</style>