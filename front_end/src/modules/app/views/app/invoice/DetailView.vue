<template>
    <div vc-page page-invoice>
        <vc-card>
            <el-breadcrumb :separator-icon="ArrowRight" class="pb-4" style="border-bottom: 1px solid #cdcdcd;" >
                <el-breadcrumb-item :to="{path: '/invoice'}">Quản lý hoá đơn</el-breadcrumb-item>
                <el-breadcrumb-item>Chi tiết hoá đơn</el-breadcrumb-item>
            </el-breadcrumb>
            <el-tabs v-model="activeName" @tab-click="handleClick">
                <div class="d-flex flex-center" style="justify-content: end;">
                    <div>
                        <vc-button type="primary" @click="onExport" :icon="'Download'">
                            {{ tl('Invoice', 'btn_export_invoice') }}
                        </vc-button>
                    </div>
                </div>
                <el-tab-pane label="Hoá đơn" name="first">
                    <vc-row :gutter="20" class="mt-4">
                        <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                            <el-descriptions border :column="1" class="" title="Shipper">
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'shipper_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.shipper.name }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Shipper', 'address_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.shipper.address }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Shipper', 'email_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.shipper.email }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Shipper', 'tel_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.shipper.tel }}
                                </el-descriptions-item>
                            </el-descriptions>

                            <el-descriptions border :column="1" style="margin-top: 44px;" title="Customer" >
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'customer_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.carton?.customer.name || '-' }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Customer', 'company_name_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.carton?.customer.company_name || '-' }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Customer', 'address_text') }}
                                        </div>
                                    </template>
                                    <div class="cell">
                                        {{ invoice?.carton?.customer.address || '-' }}
                                    </div>
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Customer', 'tax_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.carton?.customer.tax || '-' }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Customer', 'email_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.carton?.customer.email || '-' }}
                                </el-descriptions-item>
                                <el-descriptions-item>
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Customer', 'tel_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.carton?.customer.tel || '-' }}
                                </el-descriptions-item>
                            </el-descriptions>
                        </vc-col>
                        <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                            <el-descriptions border :column="1" class="" title="Hoá đơn">
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'order_no_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.order?.order_no || ''}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'invoice_no_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.invoice_no}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'invoice_date_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.invoice_date}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'shipped_per_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.shipped_per}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'shipped_date_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.shipped_date}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'total_weight_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.total_weight}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'total_volume_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.total_volumn}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'total_amount_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.carton?.total_amount || '-'}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'tax_amount_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.invoice_tax}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'total_payment_text') }}
                                        </div>
                                    </template>
                                    {{ invoice.total_payment}}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'payment_method_text') }}
                                        </div>
                                    </template>
                                    {{ invoice?.payment_method?.name || '-' }}
                                </el-descriptions-item>
                                <el-descriptions-item >
                                    <template #label>
                                        <div class="cell-item">
                                            {{ tl('Invoice', 'note_text') }}
                                        </div>
                                    </template>
                                    <div class="cell">
                                        {{ invoice.notes }}
                                    </div>
                                </el-descriptions-item>
                            </el-descriptions>
                        </vc-col>
                    </vc-row>
                </el-tab-pane>
                <el-tab-pane label="Chi tiết thùng" name="second" >
                    <el-descriptions title="Chi tiết thùng"></el-descriptions>
                    <vc-col>
                        <vc-row>
                            <vc-col :span="3" class="col-custom-header ">Carton No</vc-col>
                            <vc-col :span="3" class="col-custom-header ">Order No</vc-col>
                            <vc-col :span="3" class="col-custom-header ">Net Weight</vc-col>
                            <vc-col :span="3" class="col-custom-header ">Gross Weight</vc-col>
                            <vc-col :span="2" class="col-custom-header ">Height</vc-col>
                            <vc-col :span="2" class="col-custom-header ">Width</vc-col>
                            <vc-col :span="2" class="col-custom-header ">Length</vc-col>
                            <vc-col :span="3" class="col-custom-header ">Volume</vc-col>
                            <vc-col :span="3" class="col-custom-header ">Total Amount</vc-col>
                        </vc-row>
                        <vc-row>
                            <vc-col :span="3" class="col-custom-content ">{{ invoice?.carton?.carton_no || '-' }}</vc-col>
                            <vc-col :span="3" class="col-custom-content ">{{ invoice?.order?.order_no || '-' }}</vc-col>
                            <vc-col :span="3" class="col-custom-content ">{{ invoice?.carton?.net_weight || '-' }}</vc-col>
                            <vc-col :span="3" class="col-custom-content ">{{ invoice?.carton?.gross_weight || '-' }}</vc-col>
                            <vc-col :span="2" class="col-custom-content ">{{ invoice?.carton?.height || '-' }}</vc-col>
                            <vc-col :span="2" class="col-custom-content ">{{ invoice?.carton?.width || '-' }}</vc-col>
                            <vc-col :span="2" class="col-custom-content ">{{ invoice?.carton?.length || '-' }}</vc-col>
                            <vc-col :span="3" class="col-custom-content ">{{ invoice?.carton?.volume || '-' }}</vc-col>
                            <vc-col :span="3" class="col-custom-content ">{{ invoice?.carton?.total_amount || '-' }}</vc-col>
                        </vc-row>
                    </vc-col>
                    <el-descriptions title="Danh sách sản phẩm" class="mt-4"></el-descriptions>
                    <vc-table :datas="dataGridInvoice" :tableConfig="tableConfig" :colConfigs="colTabCartonConfig" >
                    </vc-table>
                </el-tab-pane>
            </el-tabs>

        </vc-card>
    </div>
</template>
    <script setup lang="ts">
    import { onMounted, reactive, ref } from 'vue';
    import { useRoute, useRouter } from 'vue-router';
    import tl from '@/utils/locallize';
    import invoiceService from '@app/services/app/invoice.service';
    import type { TabsPaneContext } from 'element-plus';
    import datetime from '@/utils/datetime';
    import { POPUP_TYPE } from '@/commons/const';
    import number from '@/utils/number';
    import { ArrowRight } from '@element-plus/icons-vue';
    import { colTabInvoiceConfig, colTabCartonConfig } from '@/commons/config/app/invoice.config'

    const activeName = ref('first')
    
    const route = useRoute();
    const router = useRouter();
    let _id = route.params.id as string;
    const dataGridInvoice = ref<any>([]);
    const detailRef = ref<any>();
    
    const handleClick = (tab: TabsPaneContext, event: Event) => {
    console.log(tab, event)
    }

    const tableConfig = {
        index: true,
    }

    const invoice = reactive({
        id: '',
        order: null,
        invoice_no: '',
        invoice_date: null,
        shipped_per: '',
        shipped_date: null,
        total_weight: 0,
        total_volumn: 0,
        total_amount: 0,
        payment_method: null,
        notes:'',
        invoice_tax: 0,
        total_payment: 0,
        shipper: {
            name: '',
            address: '',
            email: '',
            tel: '',
        },
        customer: {
            name: '',
            company_name: '',
            address:'',
            tax:'',
            email: '',
            tel: '',
        },
    })

    onMounted(async ()=>{
        if(_id){
            await getInvoiceDetail();
        }
    })

    const getInvoiceDetail = async () =>{
        const response = await invoiceService.detail(_id);
        Object.assign(invoice, response);
        
        const carton = response.carton;
        invoice.invoice_tax = carton.total_amount * 0.1;
        invoice.total_payment = carton.total_amount + invoice.invoice_tax;
        const carton_details = carton.carton_details;
        interface Product {
            code: string;
            image: string;
            name: string;
            origin: string;
            price_unit: any;
            unit: string;
            quantity: number;
            total_amount: number;
            category: string;
            ingredient: string;
            item_no: number;
        }
        var n = 0;
        for( var i = 0; i < carton_details.length; i++){
            //tab invoice
            var product = carton_details[i].product;
            var code = product.code;
            var image = product.image;
            var name = product.name;
            var origin = product.origin;
            var price_unit = product.price_unit;
            var quantity = carton_details[i].quantity;
            var unit = carton_details[i].unit;
            var category = product.category.name;
            var ingredient = product.ingredient;
            var total_amount = quantity*price_unit;
            n++;

            dataGridInvoice.value.push({
                code: code || '',
                name: name || '',
                image: image || '',
                origin: origin || '',
                price_unit: price_unit || '',
                unit: unit || '',
                category: category || '',
                ingredient: ingredient || '',
                quantity: quantity || 0,
                total_amount: total_amount || 0,
                item_no: n,
            } as Product);
        }
    }

    const onExport = () =>{
        invoiceService.export(_id);
    }

</script>
<style>
    .col-custom-header{
        color: #606266;
        border: 1px solid #ebeef5;
        font-weight: bold;
        background-color: #f5f6f8;
        padding: 8px 11px;
    }
    .col-custom-content{
        color: #606266;
        border: 1px solid #ebeef5;
        padding: 8px 11px;
    }
    .el-form-item {
    display: block;
    }

    .el-breadcrumb__inner {
    font-size: 2em;
    }

    .el-breadcrumb__separator {
    font-size: 2em;
    }

    .el-descriptions__label {
    width: 30%;
    }

    .el-descriptions__content {
    display: flex;
    justify-content: space-between;
    }
</style>
