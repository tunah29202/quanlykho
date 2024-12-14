<template>
    <vc-modal ref="modal" :title="modalTitle" width="99%" @close="close">
        <template #content>
            <el-form  ref="orderForm" :model="order" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right">
                <vc-row :gutter="20">
                    <vc-col :lg="4" :md="8" :sm="24" :xs="24">
                        <vc-input-group :label="tl('Order', 'order_no_text')">
                            <vc-input v-model="order_no" disabled />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="4" :md="8" :sm="24" :xs="24">
                        <vc-input-group required prop="customer_id" :label="tl('Order', 'customer_info_text')" >
                            <el-select v-model="order.customer_id" :placeholder="tl('Order', 'customer_info_holder')" >
                                <el-option v-for="customer in dataGridCustomer" :key="customer.id" :value="customer.id" 
                                :label="customer.company_name" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="4" :md="8" :sm="24" :xs="24">
                        <vc-input-group required prop="payment_method_id" :label="tl('Order', 'payment_method_text')">
                            <el-select v-model="order.payment_method_id"  :placeholder="tl('Order', 'payment_method_holder')">
                                <el-option v-for="payment_method in dataPaymentMethod" :key="payment_method.id" :value="payment_method.id" :label="payment_method.name" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="4" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="status" :label="tl('Invoice', 'status_text')">
                            <el-select v-model="order.status" :placeholder="tl('Invoice', 'customer_holder')">
                                <el-option label="Chờ xử lý" :value="0"/>
                                <el-option label="Đang xử lý" :value="1"/>
                                <el-option label="Đã giao hàng" :value="2"/>
                                <el-option label="Đã hủy" :value="3"/>
                                <el-option label="Hoàn thành" :value="4"/>
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="8" :md="12" :sm="24" :xs="24">
                        <vc-input-group prop="warehouse_names" :label="tl('Order', 'warehouse_text')" >
                            <template v-for="(item, index) in order.warehouse_names" :key="index" >
                                <el-tag class="mr-2" type="info" disable-transitions >{{ item }}</el-tag>
                            </template>
                            <el-button size="small" :icon="Plus" @click="onAddWarehouse" >{{ tl('Common', 'btn_add_new') }}</el-button>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                    
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-card style="background-color: #ececec; box-shadow: none;">
                            <div style="display: flex; margin-bottom: 20px;">
                                <el-input v-model="search" :placeholder="tl('Product', 'input_search_holder')" :prefix-icon="Search"
                                @on:keyup.enter="onSearch"/>
                                <el-button type="primary" @click="onSearch" class="ml-2">{{ tl('Common', 'btn_search') }}</el-button>
                            </div>
                            <vc-table :datas="dataGrid" :tableConfig="tableModalConfig" :colConfigs="colConfigLeft" :page="pageConfig"
                            :loading="loading" @pageChanged="onPageChanged">
                                <template #action="{data}">
                                    <div class="d-flex flex-center" style="width: 30px">
                                        <vc-button type="primary" size="small" class="btn-action" @click="onAdd(data)" :icon="'Right'">
                                        </vc-button>
                                    </div>
                                </template>
                            </vc-table>
                        </vc-card>
                    </vc-col>
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-card style="background-color: #ececec; box-shadow: none;">
                            <div class="vc-table" style="margin-top: 52px">
                                <el-table :data="dataGridRight" style="height: 436px; width: 100%;" border>
                                    <el-table-column width="50" :label="tl('Order', 'btn_remove')">
                                        <template v-slot="{row}">
                                            <div class="d-flex flex-center btn-group-action">
                                                <div class="d-flex flex-center">
                                                    <vc-button type="danger" size="small" class="btn-action" @click="onDelete(row)"
                                                    :icon="'Back'"></vc-button>
                                                </div>
                                            </div>
                                        </template>
                                    </el-table-column>
                                    <el-table-column prop="code" :label="tl('Product', 'code_text')">
                                        <template v-slot="{row}">
                                            {{ row.code }}
                                        </template>
                                    </el-table-column>
                                    <el-table-column prop="name" :label="tl('Product', 'name_text')">
                                        <template v-slot="{row}">
                                            {{ row.name }}
                                        </template>
                                    </el-table-column>
                                    <el-table-column prop="price_unit" :label="tl('Product', 'price_unit_text')">
                                        <template v-slot="{row}">
                                            {{ row.price_unit }}
                                        </template>
                                    </el-table-column>
                                    <el-table-column prop="quantity" :label="tl('Product', 'quantity_text')">
                                        <template v-slot="{row}">
                                            <el-input type="number" min="1" v-model="row.quantity"></el-input>   
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </div>
                        </vc-card>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2"
                @click="onSave(orderForm)" :loading="isLoading" :icon="'FolderChecked'">
                {{ tl("Common", props.type == POPUP_TYPE.CREATE ? "btn_add" : "btn_update") }}
            </vc-button>
        </template>
    </vc-modal>
    <vc-confirm ref="confirmDialog"></vc-confirm>
        <warehouse-modal ref="warehouseRef" @setWarehouse_ids="onSetWarehouse_ids"
        @setWarehouse_names="onSetWarehouse_names" ></warehouse-modal>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance } from "element-plus";
    import orderService from "@app/services/app/order.service";
    import { Back, Plus, Search } from '@element-plus/icons-vue';
    import { useProductStore } from '@app/stores/app/product.store';
    import { useCustomerStore } from '@app/stores/app/customer.store';
    import { usePaymentMethodStore } from '@app/stores/app/paymentMethod.store';
    import useToast from '@/components/commons/alert/vc-toast.vue';
    import { storeToRefs } from 'pinia';
    import { colConfigLeft, tableModalConfig } from '@/commons/config/app/order.config';
    import number from '@/utils/number';
    import WarehouseModal from './WarehouseModal.vue';
    import { useOrderStore } from '@app/stores/app/order.store';
    
    const storeProduct = useProductStore();
    const {dataGrid, pageConfig, search, loading} = storeToRefs(storeProduct);
    const storeOrder = useOrderStore();
    const {order_no} = storeToRefs(storeOrder);
    const storeCustomer = useCustomerStore();
    const {dataGrid: dataGridCustomer} = storeToRefs(storeCustomer);
    const storePaymentMethod = usePaymentMethodStore();
    const {dataGrid: dataPaymentMethod} = storeToRefs(storePaymentMethod);

    const rules= reactive({
        order_no: [
            { label: tl("Order", "order_no_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Order', 'order_no_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        customer_id: [
            { label: tl("Order", "customer_info_text"), required: true, validator: validate.required, trigger: ["blur"] },
        ],
        payment_method_id: [
            { label: tl("Order", "payment_method_text"), required: true, validator: validate.required, trigger: ["blur"] },
        ],
        warehouse_names: [
            { label: tl("Order", "warehouse_text"), required: true, validator: validate.required, trigger: ["blur"] },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const orderForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);
    const dataGridRight = ref<any>([]);
    const dataGridRightInit = ref<any>([]);
    const warehouseRef = ref<any>(null);


    let callback = (value: any) => { return value };

    const order = reactive({
        id: '',
        order_no: '',
        status: 0,
        order_date: null,
        total_amount: 0,
        payment_method_id: '',
        payment_method: {},
        customer_id: '',
        customer: {},
        order_details: [],
        warehouse_ids: [],
        warehouse_names: [],
    });

    onBeforeMount(async () => {
        if (order.id) await getorderDetail();
        await onSearch();
    });
    const getorderDetail = async () => {
        const response = await orderService.detail(order.id);
        Object.assign(order, response);
    };
    const onSearch = async () =>{
        await storePaymentMethod.getList();
        await storeCustomer.getList();
        let isArray : string[] = [];
        dataGridRight.value.forEach((item: any) => {
            if(item.id){
                isArray.push(item.id);
            }
        });
        await storeProduct.getProductInventory(JSON.stringify(isArray));
        await storeOrder.getOrderNo();
    }
    interface Item {
        id: string;
        code: string;
        name: string;
        product: any[]
    }
    const onAdd = async (data: Item) => {
        const exists = dataGridRight.value.some(item => item.id === data.id);
        if(!exists){
            dataGridRight.value = [...dataGridRight.value, data];
            await onSearch();
        }
    }
    const onDelete = async(data: Item) =>{
        dataGridRight.value = dataGridRight.value.filter((item: Item) => item.id !== data.id);
        await onSearch(); 
    }
    const onPageChanged = async (page: any)=>{
        pageConfig.value = {...page};
        await onSearch();
    }
    const onAddWarehouse = ()=>{
        warehouseRef.value.open(order.id)
    }
    const onSetWarehouse_ids =(value: any)=>{
        order.warehouse_ids = value
    }
    const onSetWarehouse_names =(value:any)=>{
        order.warehouse_names = value
    }
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;
            
            if(dataGridRight.value.length == 0){
                useToast.push({ message: tl('order', 'chooseProduct'), type: 'error' });
                return;
            }
            
            isLoading.value = true;
            order.order_no = order_no.value;
            var flagorderDetails = [];
            var flagTotalAmount = 0;
            for(var i = 0; i < dataGridRight.value.length; i++){
                const orderDetail : any ={
                    product_id: dataGridRight.value[i].id,
                    quantity: dataGridRight.value[i].quantity,
                    product: dataGrid.value[i],
                };
                if(order.id !== ''){
                    orderDetail.order_id = order.id;
                }
                flagorderDetails.push(orderDetail);
                flagTotalAmount += dataGridRight.value[i].quantity * number.parseCurrency(dataGridRight.value[i].price_unit);
            }
            order.total_amount = flagTotalAmount;
            order.order_details = [...flagorderDetails];
            if (order.id) {
                order.order_date = new Date(order.order_date).toLocaleString('en-EN');
                await orderService.update(order).finally(() => {
                    isLoading.value = false;
            });
            } else {
                order.order_date = new Date().toLocaleString('en-EN');
                await orderService.create(order).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let orderInfo = {
            id: '',
            order_no: '',
            status: 0,
            order_date: null,
            total_amount: 0,
            payment_method_id: '',
            payment_method: {},
            customer_id: '',
            customer: {},
            order_details: [],
            warehouse_ids: [],
            warehouse_names: [],
        };
        modalTitle.value = title;
        if (item){
            orderInfo = (await orderService.detail(item))
            const productsToAdd = orderInfo.order_details.map((detail: any) =>{
                const formattedPrice = number.parseCurrency(detail.product.price_unit);
                return {
                    ...detail.product,
                    price_unit: formattedPrice,
                    quantity: detail.quantity,
                }
            });

            dataGridRight.value = [...dataGridRight.value, ...productsToAdd];
            dataGridRightInit.value = JSON.parse(JSON.stringify(productsToAdd));
        }

        await onSearch();
        callback = _callback;
        Object.assign(order, orderInfo)
        modal.value.open();
    };

    const close = () => {
        if (orderForm.value) {
            orderForm.value.resetFields();
        }
        search.value = null;
        dataGridRight.value = [];
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
</style>