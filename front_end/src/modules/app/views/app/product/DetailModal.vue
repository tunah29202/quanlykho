<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Product", "image_text") }}
                        </div>
                    </template>
                    <el-image style="width: 100px; height: 100px;" fit="cover" :src="getUrlImage(product.image)">
                        <template #error>
                            <div class="image-slot">
                                <el-icon><icon-picture/></el-icon>
                            </div>
                        </template>
                    </el-image>
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Product", "code_text") }}
                        </div>
                    </template>
                    {{ product.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Product", "name_text") }}
                        </div>
                    </template>
                    {{ product.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "origin_text") }}
                    </div>
                </template>
                {{ product.origin }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "quantity_text") }}
                    </div>
                </template>
                {{ product.quantity }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "price_unit_text") }}
                    </div>
                </template>
                {{ product.price_unit }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "category_name_text") }}
                    </div>
                </template>
                {{ product.category ? product.category.name : '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "ingredient_text") }}
                    </div>
                </template>
                {{ product.ingredient ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "status_text") }}
                    </div>
                </template>
                {{ product.status ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Product", "warehouse_id_text") }}
                    </div>
                </template>
                {{ product.warehouse ? product.warehouse.name :'-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="productForm" :model="product" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group prop="code" :label="tl('Product', 'code_text')">
                            <vc-input v-model="product.code" :placeholder="tl('Product', 'code_holder')"/>
                        </vc-input-group>
                        <vc-input-group required prop="name" :label="tl('Product', 'name_text')">
                            <vc-input v-model="product.name" :placeholder="tl('Product', 'name_holder')" />
                        </vc-input-group>
                        <vc-input-group required prop="origin" :label="tl('Product', 'origin_text')">
                            <vc-input v-model="product.origin" :placeholder="tl('Product', 'origin_holder')" />
                        </vc-input-group>
                        <vc-input-group required prop="status" :label="tl('Product', 'status_text')">
                            <el-select v-model="product.status" :placeholder="tl('Product', 'status_holder')">
                                <el-option label="Mới" value="new"></el-option>
                                <el-option label="Cũ" value="old"></el-option>
                            </el-select>
                        </vc-input-group>
                        <vc-row :gutter="20">
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group required prop="quantity" :label="tl('Product', 'quantity_text')">
                                    <vc-input type="number" :min="1" v-model="product.quantity" :placeholder="tl('Product', 'quantity_holder')" />
                                </vc-input-group>
                            </vc-col>
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group required prop="price_unit" :label="tl('Product', 'price_unit_text')">
                                    <vc-input type="number" :min="1" v-model="product.price_unit" :placeholder="tl('Product', 'price_unit_holder')" />
                                </vc-input-group>
                            </vc-col>
                        </vc-row>
                    </vc-col>
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-row :gutter="20">
                            <vc-col :lg="21" :md="21" :sm="24" :xs="24" style="padding-right: 0px">
                                <vc-input-group prop="category_id" :label="tl('Product', 'category_name_text')">
                                    <el-select v-model="product.category_id" @change="onChangeCategory" :placeholder="tl('Product', 'category_id_holder')">
                                        <el-option v-for="item in dataGrid" :key="item.id" :label="item.name" :value="item.id"/>
                                    </el-select>
                                </vc-input-group>
                            </vc-col>
                            <vc-col :lg="3" :md="3" :sm="24" :xs="24" style="display: flex; padding-left: 0px">
                                <el-button class="ml-4" type="primary" :icon="'Plus'" circle @click="onAddNewCategory" style="margin-bottom: 18px; margin-top: auto;"/>
                            </vc-col>
                        </vc-row>
                        <vc-input-group prop="ingredient" :label="tl('Product', 'ingredient_text')">
                            <el-select v-model="product.ingredient" :placeholder="tl('Product', 'ingredient_holder')">
                                <el-option v-for="item in product.ingredient_names" :key="item" :label="item" :value="item"/>
                            </el-select>
                        </vc-input-group>
                        <vc-row :gutter="20">
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24" v-if="props.type == POPUP_TYPE.EDIT">
                                <vc-input-group :label="tl('Product', 'old_image_text')">
                                    <el-image style="width: 100px; height: 100px;" fit="cover" :src="getUrlImage(product.image)">
                                        <template #error>
                                            <div class="image-slot">
                                                <el-icon><icon-picture/></el-icon>
                                            </div>
                                        </template>
                                    </el-image>
                                </vc-input-group>
                            </vc-col>
                            <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                                <vc-input-group :label="props.type == POPUP_TYPE.EDIT ? tl('Product', 'new_image_text') : tl('Product', 'image_text')">
                                    <el-upload ref="upload" action="#" list-type="picture-card" :auto-upload="false"
                                        :on-change="handleFileChange" 
                                        :on-preview="handlePictureCardPreview" 
                                        :on-remove="handleRemove" 
                                        :limit="1" accept=".jpg,.png,.gif,.jpeg" :class="{hideUpload : !showUpload}">
                                        <el-icon> <Plus /> </el-icon>  
                                    </el-upload>
                                    <el-dialog v-model="dialogVisible">
                                        <img w-full :src="dialogImage" alt="Preview Image" />
                                    </el-dialog>
                                </vc-input-group>
                            </vc-col>
                        </vc-row>
                    </vc-col>    
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(productForm)" :loading="isLoading" :icon="'FolderChecked'">
                {{ tl("Common", props.type == POPUP_TYPE.CREATE ? "btn_add" : "btn_update") }}
            </vc-button>
        </template>
        <vc-confirm ref="confirmDialog"></vc-confirm>
    </vc-modal>
    <detail-modal ref="modalAddCategoryRef" :type="POPUP_TYPE.CREATE"></detail-modal>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance, UploadFile, UploadInstance } from "element-plus";
    import productService from "@app/services/app/product.service";
    import { useCategoryStore } from '@app/stores/app/category.store';
    import categoryService from '@app/services/app/category.service';
    import { storeToRefs } from 'pinia';
    import useToast from '@/components/commons/alert/vc-toast.vue';
    import {Plus, Picture as IconPicture} from '@element-plus/icons-vue';
    import DetailModal from '../category/DetailModal.vue';

    
    const storeCategory = useCategoryStore();
    const {dataGrid} = storeToRefs(storeCategory);

    const rules= reactive({
        code: [
            { label: tl("Product", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Product', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Product", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Product', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        origin: [
            { label: tl("Product", "origin_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Product', 'origin_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        status: [
            { label: tl("Product", "status_text"), required: true, validator: validate.required, trigger: ["blur"] },
        ],
        price_unit: [
            { label: tl("Product", "price_unit_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Product', 'price_unit_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
        quantity: [
            { label: tl("Product", "quantity_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Product', 'quantity_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
        category_id: [
            { label: tl("Product", "category_id_text"), required: true, validator: validate.required, trigger: ["blur"] },
        ],
        ingredient: [
            { label: tl("Product", "ingredient_text"), required: true, validator: validate.required, trigger: ["blur"] },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const productForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);
    const modalAddCategoryRef = ref <any>(null);
    const upload = ref<UploadInstance>();
    const showUpload = ref(true);
    const image_selected = ref<any>(null);
    const warehouse_selected = ref<any>(null);


    let callback = (value: any) => { return value };

    const product = reactive({
        id: '',
        code: null,
        name: null,
        image: null,
        origin: '',
        quantity: null,
        price_unit: null,
        category_id: '',
        category: {name: ''},
        ingredient: '',
        ingredient_names:[],
        status: '',
        warehouse_id: '',
        warehouse: {name:''},
    });

    onBeforeMount(async () => {
        await init();
        if (product.id) await getproductDetail();
    });
    const init = async () => {
        storeCategory.getList(true);
    }
    const getproductDetail = async () => {
        const response = await productService.detail(product.id);
        Object.assign(product, response);
    };
    const getUrlImage = (url : any) =>{
        var url_img = `${process.env.VITE_URL}/${url}`
        return url_img
    }
    const dialogImage = ref('');
    const dialogVisible = ref(false)
    const handleFileChange = (file: any, fileList: any) =>{
        toggleUpload();
        if(file){
            image_selected.value = file.raw;
        }
        else{
            image_selected.value = null;
        }
    }
    const handlePictureCardPreview = (file: UploadFile) =>{
        dialogImage.value = file.url!
        dialogVisible.value = true;
    }
    const handleRemove = () => {
        toggleUpload();
    }
    const toggleUpload = () =>{
        showUpload.value = !showUpload.value
    }
    const onChangeCategory = async () =>{
        product.ingredient_names = [];
        const res = await categoryService.detail(product.category_id);
        const data = res.ingredients;
        for(var i = 0; i < data.length; i++){
            product.ingredient_names.push(data[i].name)
        }
        product.ingredient = product.ingredient_names[0];
    }
    const onAddNewCategory = () => {
        modalAddCategoryRef.value.open(tl("Common", "title_modal_add", [tl("Category", "category_text")]), null, async (res: any) => {
            if(res) await storeCategory.getList(true);
        })
    }
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return; 
            isLoading.value = true;
            const warehouseSelectedJSON = localStorage.getItem('warehouse_selected')
            if(warehouseSelectedJSON != null){
                warehouse_selected.value = JSON.parse(warehouseSelectedJSON);
                product.warehouse_id = warehouse_selected.value.id;
            }
            const formData = new FormData();
            if(image_selected.value !== null){
                formData.append('image', image_selected.value);
            }
            if(product.code !== null){
                formData.append('code', product.code);
            }
            if(product.name !== null){
                formData.append('name', product.name);
            }
            if(product.origin !== null){
                formData.append('origin', product.origin);
            }
            if(product.ingredient !== null){
                formData.append('ingredient', product.ingredient);
            }
            if(product.warehouse_id !== null){
                formData.append('warehouse_id', product.warehouse_id);
            }
            if(product.category_id !== null){
                formData.append('category_id', product.category_id);
            }
            if(product.status !== null){
                formData.append('status', product.status);
            }
            if(product.quantity !== null){
                formData.append('quantity', product.quantity);
            }
            if(product.price_unit !== null){
                formData.append('price_unit', product.price_unit);
            }
            if (product.id) {
                await productService.update(product.id, formData).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await productService.create(formData).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let productInfo = {
            id: '',
            code: null,
            name: null,
            image: null,
            origin: '',
            quantity: null,
            price_unit: null,
            category_id: '',
            category: {name: ''},
            ingredient: '',
            ingredient_names: [],
            status: '',
            warehouse_id:'',
            warehouse: {name:''},
        };
        modalTitle.value = title;
        let flag = true;
        if (item){
            productInfo = (await productService.detail(item))
            if(productInfo.category_id){
                const response = await categoryService.detail(productInfo.category_id)
                const data = response.ingredients || [];
                for(var i = 0; i < data.length; i++){
                    product.ingredient_names.push(data[i].name);
                }
            }
        }
        callback = _callback;
        Object.assign(product, productInfo)
        if(!flag){
            product.category_id = '',
            product.ingredient = ''
        }
        modal.value.open();
    };

    const close = () => {
        if (upload.value) {
        upload.value!.submit()
        }
        if (productForm.value) {
            productForm.value.resetFields();
        }
        product.ingredient_names = [];
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
    .hideUpload .el-upload{
        display: none;
    }
</style>