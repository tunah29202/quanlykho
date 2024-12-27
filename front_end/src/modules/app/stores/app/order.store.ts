import { defineStore } from 'pinia'
import orderService from '@app/services/app/order.service'
import tl from '@/utils/locallize'


export const useOrderStore = defineStore('useOrderStore', {
    state: () => ({
        dataGrid: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
        category_name: <any>[],
        warehouse_id: <any>[],
        order_no: <any>[],
        pageConfig: <any>[],
        loading: false,
        dateRange_dashboard: [
            new Date(new Date().getFullYear(), new Date().getMonth(), 1),
            new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0),
        ],
        chartData: <any>{
            labels: <any>[],
            datasets: [
            {
                label: tl('Dashboard', 'number_invoice_text'),
                backgroundColor: '#205995',
                data: <any>[]
            }
            ]
        },
    }),
    getters: {
        getData(state) {
            return state.dataGrid
        },
    },
    actions: {
        async getList() {
            this.loading = true
            await orderService
                .getList({
                    sort: this.goSort,
                    search: this.search,
                    warehouse_id : this.warehouse_id,
                    category_name: this.category_name,
                    ...this.pageConfig,
                })
                .then((data) => {
                    this.dataGrid = data.data ?? []
                    this.pageConfig.total = data.total
                })
                .finally(() => {
                    this.loading = false
                })
        },
        async getNotInInvoice() {
            this.loading = true
            await orderService
                .getNotInInvoice({
                    sort: this.goSort,
                    warehouse_id : this.warehouse_id,
                    search: this.search,
                    ...this.pageConfig,
                })
                .then((data) => {
                    this.dataGrid = data.data ?? []
                    this.pageConfig.total = data.total
                })
                .finally(() => {
                    this.loading = false
                })
        },

        async delete(data: any) {
            await orderService.delete(data.id)
        },

        async getByKey(key: any) {
            this.loading = true
            await orderService
                .detail(key)
                .then((data) => {
                    this.formData = data.data ?? {}
                })
                .finally(() => {
                    this.loading = false
                })
        },

        async export() {
            this.loading = true
            await orderService
                .export({
                    sort: this.goSort,
                    is_actived: true,
                    search: this.search,
                    ...this.pageConfig,
                    size: 1000000,
                })
                .finally(() => {
                    this.loading = false
                })
        },
        setWarehouseId(item: any){
            this.warehouse_id = item;
        },
        async getOrderNo(){
            this.loading = true;

            const warehouseSelectedJson = localStorage.getItem('warehouse_selected');
            let warehouse_selected = null;
            if(warehouseSelectedJson != null){
                warehouse_selected = JSON.parse(warehouseSelectedJson);
            }
            const currentDate = new Date();
            const year = currentDate.getFullYear();
            const month = (currentDate.getMonth()+1).toString().padStart(2, '0');
            const day = currentDate.getDate().toString().padStart(2, '0');
            const formattedDate = `${year}${month}${day}`;
            if(warehouse_selected != null){
                await orderService.getOrderNo({
                    code: `ORDER-${warehouse_selected.code}-${formattedDate}-`
                })
                .then((data: any)=>{
                    const result = data.data ?? ''
                    this.order_no = result
                })
                .finally(()=>{
                    this.loading = false
                })
            }
        },
        async getStatistical(){
            this.loading = true;
            await orderService
                    .getStatistical({
                        startDate: this.dateRange_dashboard[0].toISOString(),
                        endDate: this.dateRange_dashboard[1].toISOString(),
                        warehouse_id: this.warehouse_id
                    })
                    .then((data: any)=>{
                        const result = data.data ?? [];
                        this.chartData = {
                            labels: result.labels ?? [],
                            datasets : [{
                                label: tl('Dashboard', 'number_order_text'),
                                backgroundColor: '#205995',
                                data: result.datasets ?? []
                            }]
                        }
                    })
                    .finally(()=>{
                        this.loading = false;
                    })
        },
    },
})
