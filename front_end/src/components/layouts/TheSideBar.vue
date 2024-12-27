<template>
    <el-scrollbar height="100vh" class="side-bar">
        <div v-if="!isCollapse" class="sidebar-header">
            <vc-row :gutter="20">
                <vc-col :span="6" >
                    <img src="@/assets/images/logo.jpg" style="cursor: pointer;" width="50" height="50" />
                </vc-col>
                <vc-col :span="18" style="display: grid;">
                    <el-dropdown @command="handleDropdown">
                        <span class="el-dropdown-link" style="color: white; cursor: pointer;" >
                            Chọn kho
                            <el-icon class="el-icon--right">
                                <arrow-down />
                            </el-icon>
                        </span>
                        <template #dropdown>
                            <el-scrollbar max-height="400px">
                                <el-dropdown-menu>
                                    <el-dropdown-item v-for="item in dataGridAll" :key="item.code" :command="item">
                                        {{item.name}}
                                    </el-dropdown-item>
                                </el-dropdown-menu>
                            </el-scrollbar>
                        </template>
                    </el-dropdown>
                    {{ selectedDropdownValue }}
                </vc-col>
            </vc-row>
            <hr />
        </div>
        <vc-menu :collapse="isCollapse"></vc-menu>
    </el-scrollbar>
</template>
<script setup lang="ts">
    import { storeToRefs } from "pinia";
    import { onMounted, ref, toRef } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useWarehouseStore } from "@app/stores/app/warehouse.store";
    import { useProductStore } from "@app/stores/app/product.store";
    import { useCartonStore } from "@app/stores/app/carton.store";
    import { useInvoiceStore } from "@app/stores/app/invoice.store";
    import { useOrderStore } from "@app/stores/app/order.store";

    const storeProduct = useProductStore();
    storeToRefs(storeProduct);
    const storeCarton = useCartonStore();
    storeToRefs(storeCarton);
    const storeOrder = useOrderStore();
    storeToRefs(storeOrder);
    const storeInvoice = useInvoiceStore();
    storeToRefs(storeInvoice);
    const storeWarehouse = useWarehouseStore();
    const {dataGridAll} = storeToRefs(storeWarehouse);
    const currentRoute = useRoute();
    
    const props= defineProps<{
        isCollapse?: boolean;
    }>();
    const isCollapse = toRef<any, string>(props, "isCollapse")

    const selectedDropdownValue = ref<any>(null);
    onMounted(async()=>{
        await storeWarehouse.getAll();
        const warehouseSelected = localStorage.getItem('warehouse_selected');
        if(warehouseSelected === null){
            if(dataGridAll.value.length > 0){
                localStorage.setItem('warehouse_selected', dataGridAll.value[0])
                handleDropdown(dataGridAll.value[0])
            }
        }
        else{
            handleDropdown(JSON.parse(warehouseSelected));
        }
    })
    const handleDropdown = async (item:any) =>{
        await storeWarehouse.setWarehouseSelected(item);
        selectedDropdownValue.value = item.name;
        await storeProduct.setWarehouseId(item.id);
        await storeOrder.setWarehouseId(item.id);
        await storeCarton.setWarehouseId(item.id);
        await storeInvoice.setWarehouseId(item.id);
        if(currentRoute.path =='/'){
            await storeProduct.getList();
            await storeInvoice.getList();
        }
        else if(currentRoute.path =='/product'){
            await storeProduct.getList();
        }
        else if(currentRoute.path =='/order'){
            await storeOrder.getList();
        }
        else if(currentRoute.path =='/carton'){
            await storeCarton.getList();
        }
        else if(currentRoute.path =='/invoice'){
            await storeInvoice.getList();
        }
    }

</script>
<style lang="scss">
    @import "@/assets/styles/commons/sidebar";
    .sidebar-header {
        padding: 20px 20px;
        color: white;
    }

    h1 {
        font-size: 2em;
        margin-bottom: 5px;
    }

    @media (min-width: 1024px) {
        .el-scrollbar {
            overflow: visible !important;
        }
    }
</style>