<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("customer", "customer_code_text") }}
                        </div>
                    </template>
                    {{ customer.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("customer", "full_name_text") }}
                        </div>
                    </template>
                    {{ customer.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("customer", "gender_text") }}
                    </div>
                </template>
                {{ customer.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("customer", "email_text") }}
                    </div>
                </template>
                {{ customer.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("customer", "phone_text") }}
                    </div>
                </template>
                {{ customer.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="customerForm" :model="customer" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('customer', 'customer_code_text')">
                            <vc-input v-model="customer.code" :placeholder="tl('customer', 'customer_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('customer', 'full_name_text')">
                            <vc-input v-model="customer.full_name" :placeholder="tl('customer', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('customer', 'gender_text')">
                            <el-select v-model="customer.gender" :placeholder="tl('customer', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="customer_name" :label="tl('customer', 'customer_name_text')">
                        <vc-input v-model="customer.customer_name" :placeholder="tl('customer', 'customer_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('customer', 'password_text')">
                            <vc-input v-model="customer.hashpass" type="password" show-password minlength="6" :placeholder="tl('customer', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('customer', 'email_text')">
                        <vc-input v-model="customer.email" :placeholder="tl('customer', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('customer', 'phone_text')">
                        <vc-input v-model="customer.phone" :placeholder="tl('customer', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(customerForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import customerService from "@app/services/core/customer.service";

    const rules= reactive({
        full_name: [
            { label: tl("customer", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('customer', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        customer_name: [
            { label: tl("customer", "customer_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('customer', 'customer_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("customer", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('customer', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('customer', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("customer", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("customer", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("customer", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('customer', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("customer", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('customer', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const customerForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const customer = reactive({
    id: '',
    customer_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (customer.id) await getcustomerDetail();
    });
    const getcustomerDetail = async () => {
        const response = await customerService.detail(customer.id);
        Object.assign(customer, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (customer.id) {
                await customerService.update(customer).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await customerService.create(customer).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let customerInfo = {
            id: '',
            customer_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            customerInfo = (await customerService.detail(item))
        console.log(customerInfo)
        callback = _callback;
        Object.assign(customer, customerInfo)
        modal.value.open();
    };

    const close = () => {
        if (customerForm.value) {
            customerForm.value.resetFields();
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