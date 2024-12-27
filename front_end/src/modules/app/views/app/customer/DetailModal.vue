<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" bcustomer v-if="type ==POPUP_TYPE.VIEW" border >
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Customer", "name_text") }}
                        </div>
                    </template>
                    {{ customer.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Customer", "company_name_text") }}
                    </div>
                </template>
                {{ customer.company_name }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Customer", "address_text") }}
                    </div>
                </template>
                {{ customer.address ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Customer", "tax_text") }}
                    </div>
                </template>
                {{ customer.tax ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Customer", "tel_text") }}
                    </div>
                </template>
                {{ customer.tel ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Customer", "email_text") }}
                    </div>
                </template>
                {{ customer.email ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="customerForm" :model="customer" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Customer', 'name_text')">
                            <vc-input v-model="customer.name" :placeholder="tl('Customer', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="company_name" :label="tl('Customer', 'company_name_text')">
                        <vc-input v-model="customer.company_name" :placeholder="tl('Customer', 'company_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="address" :label="tl('Customer', 'address_text')">
                        <vc-input v-model="customer.address" :placeholder="tl('Customer', 'address_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="tax" :label="tl('Customer', 'tax_text')">
                        <vc-input v-model="customer.tax" :placeholder="tl('Customer', 'tax_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row><vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="tel" :label="tl('Customer', 'tel_text')">
                        <vc-input v-model="customer.tel" :placeholder="tl('Customer', 'tel_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row><vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="email" :label="tl('Customer', 'email_text')">
                        <vc-input v-model="customer.email" :placeholder="tl('Customer', 'email_holder')" />
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
    import customerService from "@app/services/app/customer.service";

    const rules= reactive({
        name: [
            { label: tl("Customer", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Customer', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        company_name: [
            { label: tl("Customer", "company_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Customer', 'company_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        address: [
            { label: tl("Customer", "address_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Customer', 'address_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        tax: [
            { label: tl("Customer", "tax_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Customer', 'tax_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
        tel: [
            { label: tl("Customer", "tel_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Customer', 'tel_text'), validator: validate.phoneNumberRule, trigger: ["change"]},
            { label: tl('Customer', 'tel_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
        email: [
            { label: tl("Customer", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("Customer", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('Customer', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
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
        name: '',
        company_name: '',
        company_type: '',
        address: '',
        tax: null,
        tel: null,
        email: null,
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
            name: null,
            company_name: null,
            company_type: null,
            address: null,
            tax: null,
            tel: null,
            email: null,
        };
        modalTitle.value = title;
        if (item)
            customerInfo = (await customerService.detail(item))
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