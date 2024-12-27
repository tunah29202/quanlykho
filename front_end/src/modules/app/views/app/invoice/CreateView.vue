<template>
    <div vc-page page-invoice>
        <vc-card >
            <el-breadcrumb :separator-icon="ArrowRight" class="pb-4" style="border-bottom: 1px solid #cdcdcd;" >
                <el-breadcrumb-item :to="{path: '/invoice'}">{{ tl('SideBar', 'invoice_management') }}</el-breadcrumb-item>
                <el-breadcrumb-item>{{_id ? tl('Invoice', 'edit_text') :tl('Invoice', 'add_new_text')}}</el-breadcrumb-item>
            </el-breadcrumb>
            <el-form :model="invoice" ref="invoiceForm" :rules="rules" label-position="right"
             style="padding: 12px 16px;" require-asterisk-position="right" >
                <vc-row :gutter="20">
                    <vc-col :lg="6" :md="12" :sm="24" :xs=24 >
                        <vc-input-group :label="tl('Invoice', 'invoice_no_text')">
                            <vc-input v-model="invoice_no" disabled />
                        </vc-input-group>
                        <vc-input-group required prop="carton_no" :label="tl('Invoice', 'carton_no_text')">
                            <el-input v-model="invoice.carton_no" readonly style="max-width: 600px;" class="input-with-select" >
                                <template #prepend>
                                    <el-button @click="onSelectCarton" > {{ tl('Invoice', 'carton_no_holder') }} </el-button>
                                </template>
                            </el-input>
                        </vc-input-group>
                        <vc-input-group required prop="order_no" :label="tl('Invoice', 'order_no_text')">
                            <el-input v-model="invoice.order_no" readonly style="max-width: 600px;" class="input-with-select" >
                                <template #prepend>
                                    <el-button @click="onSelectOrder" > {{ tl('Invoice', 'order_no_holder') }} </el-button>
                                </template>
                            </el-input>
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="6" :md="12" :sm="24" :xs=24 >
                        <vc-input-group :label="tl('Invoice', 'total_weight_text')">
                            <vc-input v-model="invoice.total_weight" disabled />
                        </vc-input-group>
                        <vc-input-group :label="tl('Invoice', 'total_volumn_text')">
                            <vc-input v-model="invoice.total_volumn" disabled />
                        </vc-input-group>
                        <vc-input-group :label="tl('Invoice', 'total_amount_text')">
                            <vc-input v-model="invoice.total_amount" disabled />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="6" :md="12" :sm="24" :xs=24 >
                        <vc-input-group required prop="shipped_per" :label="tl('Invoice', 'shipped_per_text')">
                            <vc-input v-model="invoice.shipped_per" :placeholder="tl('Invoice', 'shipped_per_holder')"></vc-input>
                        </vc-input-group>
                        <vc-input-group required prop="shipper_id" :label="tl('Invoice', 'shipper_text')">
                            <el-select v-model="invoice.shipper_id" @change="onChangeShipper" :placeholder="tl('Invoice', 'shipper_holder')">
                                <el-option v-for="shipper in dataShipper" :key="shipper.id" :value="shipper.id" :label="shipper.name" />
                            </el-select>
                        </vc-input-group>
                        <vc-input-group required prop="shipped_date" :label="tl('Invoice', 'shipped_date_text')">
                            <el-date-picker v-model="invoice.shipped_date" type="datetime" format="YYYY/MM/DD hh:mm:ss"
                            :placeholder="tl('Invoice', 'shipped_date_holder')" :disabled-date="disabledDate" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="6" :md="12" :sm="24" :xs=24 >
                        <vc-input-group required prop="payment_method_id" :label="tl('Invoice', 'payment_method_text')">
                            <el-select v-model="invoice.payment_method_id"  :placeholder="tl('Invoice', 'payment_method_holder')">
                                <el-option v-for="payment_method in dataPaymentMethod" :key="payment_method.id" :value="payment_method.id" :label="payment_method.name" />
                            </el-select>
                        </vc-input-group>
                        <vc-input-group required prop="notes" :label="tl('Invoice', 'notes_text')">
                            <vc-input :rows="1" type="textarea" v-model="invoice.notes" :placeholder="tl('Invoice', 'notes_holder')"></vc-input>
                        </vc-input-group>
                        <vc-input-group v-if="_id" required prop="status" :label="tl('Invoice', 'status_text')">
                            <el-select v-model="invoice.status" :placeholder="tl('Invoice', 'customer_holder')">
                                <el-option :label="tl('Invoice', 'not_exported_text')" :value="0"/>
                                <el-option :label="tl('Invoice', 'exported_text')" :value="1"/>
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>

            <vc-row :gutter="20" style="padding: 12px 16px;">
                <vc-col v-if="invoice.customer_id" :lg="12" :md=12 :sm="24" :xs="24" >
                    <el-descriptions class="mt-4" border :column="1" title="Khách hàng">
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Customer', 'name_text') }}
                                </div>
                            </template>
                            {{ customer.name }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Customer', 'company_name_text') }}
                                </div>
                            </template>
                            {{ customer.company_name }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Customer', 'address_text') }}
                                </div>
                            </template>
                            {{ customer.address }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Customer', 'email_text') }}
                                </div>
                            </template>
                            {{ customer.email }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Customer', 'tel_text') }}
                                </div>
                            </template>
                            {{ customer.tel }}
                        </el-descriptions-item>
                    </el-descriptions>
                </vc-col>
                <vc-col v-if="invoice.shipper_id" :lg="12" :md=12 :sm="24" :xs="24" >
                    <el-descriptions class="mt-4" border :column="1" title="Đơn vị vận chuyển">
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Invoice', 'shipper_text') }}
                                </div>
                            </template>
                            {{ shipper.name }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Shipper', 'address_text') }}
                                </div>
                            </template>
                            {{ shipper.address }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Shipper', 'email_text') }}
                                </div>
                            </template>
                            {{ shipper.email }}
                        </el-descriptions-item>
                        <el-descriptions-item>
                            <template #label>
                                <div class="cell-item">
                                    {{ tl('Shipper', 'tel_text') }}
                                </div>
                            </template>
                            {{ shipper.tel }}
                        </el-descriptions-item>
                    </el-descriptions>
                </vc-col>
            </vc-row>
            <div class="d-flex align-center" style="justify-content: end; padding: 16px" >
                <div>
                    <vc-button type="primary" :loading="isLoading" class="btn-close" @click=onSave(invoiceForm) >
                        {{  _id ? tl('Common', 'btn_update') : tl('Common', 'btn_add') }}
                    </vc-button>
                    <vc-button type="info" @click="onClickBack">
                            {{ tl('Common', 'btn_exit') }}
                    </vc-button>
                </div>
            </div>
        </vc-card>
        <vc-confirm ref="confirmDialog"></vc-confirm>
        <order-modal ref="detailOrderRef" @setOrder="onSetOrder"></order-modal>
        <carton-modal ref="detailCartonRef" @setCarton="onSetCarton"></carton-modal>
    </div>
</template>
    <script setup lang="ts">
    import { onMounted, reactive, ref, watchEffect } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import tl from '@/utils/locallize';
    import invoiceService from '@app/services/app/invoice.service';
    import type { TabsPaneContext } from 'element-plus';
    import datetime from '@/utils/datetime';
    import { POPUP_TYPE } from '@/commons/const';
    import number from '@/utils/number';
    import { ArrowRight } from '@element-plus/icons-vue';
    import { colTabInvoiceConfig, colTabCartonConfig } from '@/commons/config/app/invoice.config'
    import type { FormInstance } from 'element-plus';
    import validate from '@/utils/validate';
    import { useShipperStore } from '@app/stores/app/shipper.store';
    import { usePaymentMethodStore } from '@app/stores/app/paymentMethod.store';
    import { useInvoiceStore } from '@app/stores/app/invoice.store';
    import { storeToRefs } from 'pinia';
    import OrderModal from './OrderModal.vue';
    import CartonModal from './CartonModal.vue';
    import shipperService from '@app/services/app/shipper.service';

    const invoiceForm = ref<FormInstance>();
    const route = useRoute();
    const router = useRouter();
    let _id = route.params.id as string;
    const confirmDialog = ref<any>(null);
    const detailOrderRef = ref<any>(null);
    const detailCartonRef = ref<any>(null);
    const isLoading = ref(false);
    const title = ref<any>(null);

    const storeShipper = useShipperStore();
    const {dataGrid: dataShipper} = storeToRefs(storeShipper);
    const storePaymentMethod = usePaymentMethodStore();
    const {dataGrid: dataPaymentMethod} = storeToRefs(storePaymentMethod);
    const storeInvoice = useInvoiceStore();
    const {warehouse_id, invoice_no} = storeToRefs(storeInvoice)
    
    onMounted(async ()=>{
        await storeShipper.getList(true);
        await storePaymentMethod.getList(true);
        if(_id) await getInvoiceDetail();
        else await storeInvoice.getInvoiceNo();     
    })

    watchEffect(async ()=>{
        _id = route.params.id as string;
        if(_id){
            invoice.id = _id
            title.value = tl('Invoice', 'title_edit_text')
        }
        else{
            title.value = tl('Invoice', 'title_create_text')
        }
    })

    const getInvoiceDetail = async ()=>{
        const res = await invoiceService.detail(_id);
        Object.assign(invoice, res);
        invoice_no.value = invoice.invoice_no;
        const carton = invoice.carton
        invoice.carton_id = carton?.id
        invoice.carton_no = carton?.carton_no
        invoice.total_weight = carton?.gross_weight.toString()
        invoice.total_amount = carton?.total_amount
        invoice.total_volumn = carton?.volume
        invoice.customer_id = carton?.customer?.id
        invoice.customer = carton?.customer
        const order = invoice.order
        invoice.order_id = order?.id
        invoice.order_no = order?.order_no
        Object.assign(customer, carton.customer)
        Object.assign(shipper, invoice.shipper)
    }
    
    let shipper = reactive({
        id: '',
        name: null,
        address: null,
        email: null,
        tel: null,
        fax: null,
    })
    const onChangeShipper = async ()=>{
        let shipperInfo = {
            id: '',
            name: null,
            address: null,
            email: null,
            tel: null,
            fax: null,
        };
        shipperInfo = await shipperService.detail(invoice.shipper_id);
        Object.assign(shipper, shipperInfo);
    }
    
    const onClickBack = () =>{
        router.push({name: 'InvoiceList'})
    }
    let customer = reactive({
        id: '',
        name: null,
        company_name: null,
        address: null,
        tax: null,
        email: null,
        tel: null,
        fax: null,
    })

    const rules= reactive({
    invoice_no: [
        { label: tl("Invoice", "invoice_no_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    invoice_date: [
        { label: tl("Invoice", "invoice_date_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    shipped_per: [
        { label: tl("Invoice", "shipped_per_text"), required: true, validator: validate.required, trigger: ["blur"] },
        { label: tl('Invoice', 'shipped_per_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
    ],
    shipped_date: [
        { label: tl("Invoice", "shipped_date_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    shipper_id: [
        { label: tl("Invoice", "shipper_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    payment_method_id: [
        { label: tl("Invoice", "payment_method_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    order_no: [
        { label: tl("Invoice", "order_no_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    carton_no: [
        { label: tl("Invoice", "carton_no_text"), required: true, validator: validate.required, trigger: ["blur"] },
    ],
    });
    
    const invoice = reactive({
        id: '',
        invoice_no: '',
        order_id: '',
        order_no: '',
        carton_id: '',
        carton_no: '',
        invoice_date: null,
        shipped_per: '',
        shipped_date: null,
        total_weight: '0',
        total_volumn: 0,
        total_amount: 0,
        notes:'',
        status: 0,
        shipper_id:'',
        shipper: {},
        payment_method_id:'',
        payment_method: {},
        customer_id:'',
        customer: {},
        warehouse_id:'',
    })

    const onSelectOrder = () =>{
        detailOrderRef.value.open();
    }

    const onSelectCarton = () =>{
        detailCartonRef.value.open();
    }

    const onSetCarton = (value: any) =>{
        invoice.carton_id = value.id
        invoice.carton_no = value.carton_no
        invoice.total_weight = value.gross_weight.toString()
        invoice.total_amount = value.total_amount
        invoice.total_volumn = value.volume
        invoice.customer_id = value.customer.id
        invoice.customer = value.customer
        Object.assign(customer, invoice.customer)
    }
    const onSetOrder = (value: any) =>{
        invoice.order_id = value.id
        invoice.order_no = value.order_no
    }
    const disabledDate = (time: Date) => {
        const currentDate = new Date();
        currentDate.setHours(0, 0, 0, 0);
        return time.getTime() < currentDate.getTime();
    }
    const onSave = async (formEl: FormInstance | undefined) =>{
        if(!formEl)
            return;
        await formEl.validate(async (valid) =>{
            if(!valid)
                return;
            
            isLoading.value = true;
            invoice.warehouse_id = warehouse_id.value;
            invoice.invoice_no = invoice_no.value;
            invoice.shipped_date = new Date(invoice.shipped_date).toLocaleString('en-EN');
            if(invoice.id){
                invoice.invoice_date = new Date(invoice.invoice_date).toLocaleString('en-EN');
                await invoiceService.update(invoice).finally(()=>{
                    isLoading.value = false;
                    setTimeout(() => {
                        router.push({
                            name: 'InvoiceList',
                        });
                    }, 500);
                })
            }
            else{
                invoice.invoice_date = new Date().toLocaleString('en-EN');
                await invoiceService.create(invoice).finally(()=>{
                    isLoading.value = false;
                    setTimeout(() => {
                        router.push({
                            name: 'InvoiceList',
                        });
                    }, 500);
                })
            }
        })
    }


</script>
<style>
    .el-form-item {
    display: block;
    }

    .el-breadcrumb__inner {
    font-size: 2em;
    }

    .el-breadcrumb__separator {
    font-size: 2em;
    }
</style>
