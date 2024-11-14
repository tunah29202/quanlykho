<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("category", "category_code_text") }}
                        </div>
                    </template>
                    {{ category.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("category", "full_name_text") }}
                        </div>
                    </template>
                    {{ category.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("category", "gender_text") }}
                    </div>
                </template>
                {{ category.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("category", "email_text") }}
                    </div>
                </template>
                {{ category.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("category", "phone_text") }}
                    </div>
                </template>
                {{ category.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="categoryForm" :model="category" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('category', 'category_code_text')">
                            <vc-input v-model="category.code" :placeholder="tl('category', 'category_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('category', 'full_name_text')">
                            <vc-input v-model="category.full_name" :placeholder="tl('category', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('category', 'gender_text')">
                            <el-select v-model="category.gender" :placeholder="tl('category', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="category_name" :label="tl('category', 'category_name_text')">
                        <vc-input v-model="category.category_name" :placeholder="tl('category', 'category_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('category', 'password_text')">
                            <vc-input v-model="category.hashpass" type="password" show-password minlength="6" :placeholder="tl('category', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('category', 'email_text')">
                        <vc-input v-model="category.email" :placeholder="tl('category', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('category', 'phone_text')">
                        <vc-input v-model="category.phone" :placeholder="tl('category', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(categoryForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import categoryService from "@app/services/core/category.service";

    const rules= reactive({
        full_name: [
            { label: tl("category", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('category', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        category_name: [
            { label: tl("category", "category_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('category', 'category_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("category", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('category', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('category', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("category", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("category", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("category", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('category', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("category", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('category', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const categoryForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const category = reactive({
    id: '',
    category_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (category.id) await getcategoryDetail();
    });
    const getcategoryDetail = async () => {
        const response = await categoryService.detail(category.id);
        Object.assign(category, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (category.id) {
                await categoryService.update(category).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await categoryService.create(category).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let categoryInfo = {
            id: '',
            category_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            categoryInfo = (await categoryService.detail(item))
        console.log(categoryInfo)
        callback = _callback;
        Object.assign(category, categoryInfo)
        modal.value.open();
    };

    const close = () => {
        if (categoryForm.value) {
            categoryForm.value.resetFields();
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