<template>
    <div class="vc-page page-home">
        <el-row :gutter="20">
            <el-col :span="6">
                <div class="statistic-card" style="background-color: #F56C6C; display: flex; align-items: center;" >
                    <div>
                        <User style="width: 120; height: 120; color: white;"/>
                    </div>
                    <el-statistic :value="pageConfigCustomer.total">
                        <template #title>
                            <span>{{ tl('Dashboard', 'total_customer_text') }}</span>
                            <el-tooltip effect="dark" :content="tl('Dashboard', 'total_customer_tooltip')" placement="top">
                                <el-icon style="margin-left: 4px;" :size="12">
                                    <Warning />
                                </el-icon>
                            </el-tooltip>
                        </template>
                    </el-statistic>
                </div>
            </el-col>
            <el-col :span="6">
                <div class="statistic-card" style="background-color: #E6A23C; display: flex; align-items: center;" >
                    <div>
                        <Place style="width: 120; height: 120; color: white;"/>
                    </div>
                    <el-statistic :value="pageConfigOrder.total">
                        <template #title>
                            <span>{{ tl('Dashboard', 'total_order_text') }}</span>
                            <el-tooltip effect="dark" :content="tl('Dashboard', 'total_order_tooltip')" placement="top">
                                <el-icon style="margin-left: 4px;" :size="12">
                                    <Warning />
                                </el-icon>
                            </el-tooltip>
                        </template>
                    </el-statistic>
                </div>
            </el-col>
            <el-col :span="6">
                <div class="statistic-card" style="background-color: #409EFF; display: flex; align-items: center;" >
                    <div>
                        <Tickets style="width: 120; height: 120; color: white;"/>
                    </div>
                    <el-statistic :value="pageConfigInvoice.total">
                        <template #title>
                            <span>{{ tl('Dashboard', 'total_invoice_text') }}</span>
                            <el-tooltip effect="dark" :content="tl('Dashboard', 'total_invoice_tooltip')" placement="top">
                                <el-icon style="margin-left: 4px;" :size="12">
                                    <Warning />
                                </el-icon>
                            </el-tooltip>
                        </template>
                    </el-statistic>
                </div>
            </el-col>
            <el-col :span="6">
                <div class="statistic-card" style="background-color: #67C23A; display: flex; align-items: center;" >
                    <div>
                        <TakeawayBox style="width: 120; height: 120; color: white;"/>
                    </div>
                    <el-statistic :value="pageConfigProduct.total">
                        <template #title>
                            <span>{{ tl('Dashboard', 'total_product_text') }}</span>
                            <el-tooltip effect="dark" :content="tl('Dashboard', 'total_product_tooltip')" placement="top">
                                <el-icon style="margin-left: 4px;" :size="12">
                                    <Warning />
                                </el-icon>
                            </el-tooltip>
                        </template>
                    </el-statistic>
                </div>
            </el-col>
        </el-row>
        <vc-card class="mb-4 pa-4" style="margin-top: 40px;">
            <el-row :gutter="20">
                <el-col :span="12">
                    <div style="display: flex">
                        <div>
                            <b class="mr-2">{{ tl('Dashboard', 'date_text') }}</b>
                            <el-date-picker @change="handleChangeDateRange" v-model="dateRange_dashboard" type="daterange" 
                            start-placeholder="Start Date" end-placeholder="End Date"/>
                        </div>
                    </div>
                    <h1 class="mb-4 mt-4">{{ tl('Dashboard', 'statistical_text') }}</h1>
                    <div class="chart-container">
                        <Bar :data="chartData" :options="options" />
                    </div>
                </el-col>
                <el-col :span="12">
                    <div style="display: flex">
                        <div>
                            <b class="mr-2">{{ tl('Dashboard', 'date_text') }}</b>
                            <el-date-picker @change="handleChangeOrderDateRange" v-model="dateRange_Order" type="daterange" 
                            start-placeholder="Start Date" end-placeholder="End Date"/>
                        </div>
                    </div>
                    <h1 class="mb-4 mt-4">{{ tl('Dashboard', 'order_statistical_text') }}</h1>
                    <div class="chart-container">
                        <Bar :data="chartOrder" :options="options" />
                    </div>
                </el-col>
            </el-row>
        </vc-card>
    </div>
</template>
<script setup lang="ts">
    import { onMounted } from 'vue';
    import tl from '@/utils/locallize';
    import { Place, TakeawayBox, Tickets, User, Warning } from '@element-plus/icons-vue';
    import { useInvoiceStore } from '@app/stores/app/invoice.store';
    import { useProductStore } from '@app/stores/app/product.store';
    import { useCustomerStore } from '@app/stores/app/customer.store';
    import { useOrderStore } from '@app/stores/app/order.store';
    import { storeToRefs } from 'pinia';
    import {Bar} from 'vue-chartjs';
    import {
    Chart as ChartJS,
    CategoryScale, 
    LinearScale, 
    BarElement,  
    Title,       
    Tooltip,   
    Legend      
    } from 'chart.js';
    ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

    const storeInvoice = useInvoiceStore();
    const {pageConfig: pageConfigInvoice, dateRange_dashboard, chartData} = storeToRefs(storeInvoice);
    const storeProduct = useProductStore();
    const {pageConfig: pageConfigProduct} = storeToRefs(storeProduct);
    const storeCustomer = useCustomerStore();
    const {pageConfig: pageConfigCustomer} = storeToRefs(storeCustomer);
    const storeOrder = useOrderStore();
    const {pageConfig: pageConfigOrder, dateRange_dashboard: dateRange_Order, chartData: chartOrder} = storeToRefs(storeOrder);

    onMounted(async ()=>{
        await storeProduct.getList();
        await storeCustomer.getList();
        await storeOrder.getList();
        await storeInvoice.getList();
        await storeInvoice.getStatistical();
        await storeOrder.getStatistical();
    })
    const handleChangeDateRange = async () =>{
        await storeInvoice.getStatistical();
    }
    const handleChangeOrderDateRange = async () =>{
        await storeOrder.getStatistical();
    }
    const options = {
    responsive: true,
    maintainAspectRatio: false
    }
</script>
<style>

.chart-container {
  position: relative;
  margin: auto;
  height: 50vh;
}

:global(h2#card-usage ~ .example .example-showcase) {
  background-color: var(--el-fill-color) !important;
}

.el-statistic {
  --el-statistic-content-font-size: 28px;
}

.statistic-card {
  height: 100%;
  padding: 10px;
  border-radius: 4px;
  background-color: var(--el-bg-color-overlay);
}

.statistic-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  font-size: 12px;
  color: var(--el-text-color-regular);
  margin-top: 16px;
}

.statistic-footer .footer-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.statistic-footer .footer-item span:last-child {
  display: inline-flex;
  align-items: center;
  margin-left: 4px;
}

.green {
  color: var(--el-color-success);
}

.red {
  color: var(--el-color-error);
}

.el-statistic__number {
  font-size: 48px;
  color: white
}

.el-statistic__head div {
  color: white;
  display: inline-flex;
  align-items: center;
}
</style>