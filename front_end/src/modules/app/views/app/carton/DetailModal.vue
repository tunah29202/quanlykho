<template>
    <vc-modal ref="modal" :title="modalTitle" width="99%" @close="close">
        <template #content>
            <el-form  ref="cartonForm" :model="carton" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right">
                <vc-row :gutter="20">
                    <vc-col :lg="8" :md="8" :sm="24" :xs="24">
                        <vc-input-group prop="carton_no" :label="tl('Carton', 'carton_no_text')">
                            <vc-input v-model="carton.carton_no" :placeholder="tl('Carton', 'carton_no_holder')"/>
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg="8" :md="8" :sm="24" :xs="24">
                        <vc-row :gutter="20">
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group prop="width" :label="tl('Carton', 'width_text')">
                                    <vc-input type="number" v-model="carton.width" :placeholder="tl('Carton', 'width_holder')" />
                                </vc-input-group>
                            </vc-col>
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group prop="height" :label="tl('Carton', 'height_text')">
                                    <vc-input type="number" v-model="carton.height" :placeholder="tl('Carton', 'height_holder')" />
                                </vc-input-group>
                            </vc-col>
                        </vc-row>
                    </vc-col>
                    <vc-col :lg="8" :md="8" :sm="24" :xs="24">
                        <vc-row :gutter="20">
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group prop="length" :label="tl('Carton', 'length_text')">
                                    <vc-input type="number" v-model="carton.length" :placeholder="tl('Carton', 'length_holder')" />
                                </vc-input-group>
                            </vc-col>
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group prop="gross_weight" :label="tl('Carton', 'gross_weight_text')">
                                    <vc-input type="number" v-model="carton.gross_weight" :placeholder="tl('Carton', 'gross_weight_holder')" />
                                </vc-input-group>
                            </vc-col>
                        </vc-row>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-card style="background-color: #ececec; box-shadow: none;">
                            <div style="display: flex; margin-bottom: 20px;">
                                <el-input v-model="search" :placeholder="tl('Product', 'input_search_holder')" :prefix-icon="Search"
                                @on:keyup.enter="onSearch"/>
                                <el-input v-model="category_name" :placeholder="tl('Product', 'category_search_holder')" :prefix-icon="Search"
                                @on:keyup.enter="onSearch"/>
                                <el-button type="primary" @click="onSearch" class="ml-2">{{ tl('Common', 'btn-search') }}</el-button>
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
                                    <el-table-column width="50" :label="tl('Carton', 'btn_remove')">
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
                                    <el-table-column prop="quantity" :label="tl('Product', 'quantity_text')">
                                        <template v-slot="{row}">
                                            <el-input v-model="row.unit"></el-input>   
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
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" carton_no="F00004"
                @click="onSave(cartonForm)" :loading="isLoading" :icon="'FolderChecked'">
                {{ tl("Common", props.type == POPUP_TYPE.CREATE ? "btn_add" : "btn_update") }}
            </vc-button>
        </template>
        <vc-confirm ref="confirmDialog"></vc-confirm>
    </vc-modal>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance } from "element-plus";
    import cartonService from "@app/services/app/carton.service";
    import productService from "@app/services/app/product.service";
    import { Back, Search } from '@element-plus/icons-vue';
    import { useProductStore } from '@app/stores/app/product.store';
    import { useToast } from '@/components/commons/alert/vc-toast.vue';
    import { storeToRefs } from 'pinia';
    import { colConfigLeft, tableModalConfig } from '@/commons/config/app/carton.config';
    import number from '@/utils/number';
    
    const store = useProductStore();
    const {dataGrid, pageConfig, search, loading, category_name} = storeToRefs(store);
    const rules= reactive({
        carton_no: [
            { label: tl("carton", "carton_no_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('carton', 'carton_no_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        net_weight: [
            { label: tl("carton", "net_weight_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('carton', 'net_weight_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        gross_weight: [{ label: tl("carton", "gross_weight_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        length: [
            { label: tl("carton", "length_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('carton', 'length_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        width: [
            { label: tl('carton', 'width_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const cartonForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);
    const dataGridRight = ref<any>([]);
    const dataGridRightInit = ref<any>([]);

    let callback = (value: any) => { return value };

    const carton = reactive({
        id: '',
        carton_no: '',
        net_weight: 0,
        gross_weight: 0,
        length: 0,
        width: 0,
        height: 0,
        volume: 0,
        total_amount: 0,
        warehouse_id: '',
        customer_company: '',
        carton_details: []
    });

    onBeforeMount(async () => {
        if (carton.id) await getcartonDetail();
        await onSearch();
    });
    const getcartonDetail = async () => {
        const response = await cartonService.detail(carton.id);
        Object.assign(carton, response);
    };
    const onSearch = async () =>{
        await store.getList();
    }
    interface Item {
        id: string;
        code: string;
        name: string;
        warehouse_id: string;
        product: any[]
    }
    const onAdd = async (data: Item) => {
        const exists = dataGridRight.value.some(item => item.id === data.id);
        if(!exists){
            const newData = {...data, unit:"pcs",}
            dataGridRight.value = [...dataGridRight.value, newData];
            await onSearch();
        }
        else {
            useToast.push({ message: "Product đã tồn tại trong carton!", type: 'error' })
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
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;
            
            if(dataGridRight.value.length == 0){
                useToast.push({ message: tl('Carton', 'chooseProduct'), type: 'error' });
                return;
            }
            
            isLoading.value = true;

            carton.net_weight = Number((carton.gross_weight*0.96).toFixed(2));
            carton.total_amount = Number(((carton.height*carton.width*carton.length)/1000000).toFixed(2));
            carton.warehouse_id = '';

            var flagCartonDetails = [];
            var flagTotalAmount = 0;
            for(var i = 0; i < dataGridRight.value.length; i++){
                const cartonDetail : any ={
                    product_id: dataGridRight.value[i].id,
                    quantity: dataGridRight.value[i].quantity,
                    unit: dataGridRight.value[i].unit,
                    product: dataGrid.value[i],
                };
                if(carton.id !== ''){
                    cartonDetail.carton_id = carton.id;
                }
                flagCartonDetails.push(cartonDetail);
                flagTotalAmount += dataGridRight.value[i].quantity * number.parseCurrency(dataGridRight.value[i].price_unit);
            }
            carton.total_amount = flagTotalAmount;
            carton.carton_details = [...flagCartonDetails];

            if (carton.id) {
                await cartonService.update(carton).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await cartonService.create(carton).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let cartonInfo = {
            id: '',
            carton_no: '',
            net_weight: 0,
            gross_weight: 0,
            length: 0,
            width: 0,
            height: 0,
            volume: 0,
            total_amount: 0,
            warehouse_id: '',
            customer_company: '',
            carton_details: []
        };
        modalTitle.value = title;
        if (item){
            cartonInfo = (await cartonService.detail(item))
            const productsToAdd = cartonInfo.carton_details.map((detail: any) =>{
                const formattedPrice = number.parseCurrency(detail.product.price_unit);
                return {
                    ...detail.product,
                    price_unit: formattedPrice,
                    quantity: detail.quantity,
                    unit: detail.unit
                }
            });

            dataGridRight.value = [...dataGridRight.value, ...productsToAdd];
            dataGridRightInit.value = JSON.parse(JSON.stringify(productsToAdd));
        }

        await onSearch();
        callback = _callback;
        Object.assign(carton, cartonInfo)
        modal.value.open();
    };

    const close = () => {
        if (cartonForm.value) {
            cartonForm.value.resetFields();
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