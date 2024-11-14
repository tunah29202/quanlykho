<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("product", "product_code_text") }}
                        </div>
                    </template>
                    {{ product.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("product", "full_name_text") }}
                        </div>
                    </template>
                    {{ product.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("product", "gender_text") }}
                    </div>
                </template>
                {{ product.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("product", "email_text") }}
                    </div>
                </template>
                {{ product.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("product", "phone_text") }}
                    </div>
                </template>
                {{ product.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="productForm" :model="product" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('product', 'product_code_text')">
                            <vc-input v-model="product.code" :placeholder="tl('product', 'product_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('product', 'full_name_text')">
                            <vc-input v-model="product.full_name" :placeholder="tl('product', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('product', 'gender_text')">
                            <el-select v-model="product.gender" :placeholder="tl('product', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="product_name" :label="tl('product', 'product_name_text')">
                        <vc-input v-model="product.product_name" :placeholder="tl('product', 'product_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('product', 'password_text')">
                            <vc-input v-model="product.hashpass" type="password" show-password minlength="6" :placeholder="tl('product', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('product', 'email_text')">
                        <vc-input v-model="product.email" :placeholder="tl('product', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('product', 'phone_text')">
                        <vc-input v-model="product.phone" :placeholder="tl('product', 'phone_holder')" />
                        </vc-input-group>
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
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance } from "element-plus";
    import productService from "@app/services/core/product.service";

    const rules= reactive({
        full_name: [
            { label: tl("product", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('product', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        product_name: [
            { label: tl("product", "product_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('product', 'product_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("product", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('product', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('product', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("product", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("product", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("product", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('product', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("product", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('product', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
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

    let callback = (value: any) => { return value };

    const product = reactive({
    id: '',
    product_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (product.id) await getproductDetail();
    });
    const getproductDetail = async () => {
        const response = await productService.detail(product.id);
        Object.assign(product, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (product.id) {
                await productService.update(product).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await productService.create(product).finally(() => {
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
            product_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            productInfo = (await productService.detail(item))
        console.log(productInfo)
        callback = _callback;
        Object.assign(product, productInfo)
        modal.value.open();
    };

    const close = () => {
        if (productForm.value) {
            productForm.value.resetFields();
        }
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