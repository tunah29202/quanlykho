import { defineStore } from 'pinia'
import invoiceService from '@app/services/app/invoice.service'
import tl from '@/utils/locallize'

export const useInvoiceStore = defineStore('useInvoiceStore', {
    state: () => ({
        dataGrid: <any>[],
        formData: <any>{},
        goSort: <any>[],
        search: <any>[],
        dateRange_dashboard: [
            new Date(new Date().getFullYear(), new Date().getMonth(), 1),
            new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0),
        ],
        pageConfig: <any>[],
        warehouse_id: <any>[],
        invoice_no: <any>[],
        loading: false,
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
            await invoiceService
                .getList({
                    warehouse_id : this.warehouse_id,
                    sort: this.goSort,
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
            await invoiceService.delete(data.id)
        },

        setWarehouseId(item: any){
            this.warehouse_id = item;
        },
        async getInvoiceNo(){
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
                await invoiceService.getInvoiceNo({
                    code: `INVOICE-${warehouse_selected.code}-${formattedDate}-`
                })
                .then((data: any)=>{
                    const result = data.data ?? ''
                    this.invoice_no = result
                })
                .finally(()=>{
                    this.loading = false
                })
            }
        },
        async getStatistical(){
            this.loading = true;
            await invoiceService
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
                                label: tl('Dashboard', 'number_invoice_text'),
                                backgroundColor: '#205995',
                                data: result.datasets ?? []
                            }]
                        }
                    })
                    .finally(()=>{
                        this.loading = false;
                    })
        },

        async getByKey(key: any) {
            this.loading = true
            await invoiceService
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
            await invoiceService
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
    },
})
