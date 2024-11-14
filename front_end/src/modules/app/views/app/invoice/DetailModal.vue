<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("invoice", "invoice_code_text") }}
                        </div>
                    </template>
                    {{ invoice.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("invoice", "full_name_text") }}
                        </div>
                    </template>
                    {{ invoice.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("invoice", "gender_text") }}
                    </div>
                </template>
                {{ invoice.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("invoice", "email_text") }}
                    </div>
                </template>
                {{ invoice.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("invoice", "phone_text") }}
                    </div>
                </template>
                {{ invoice.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="invoiceForm" :model="invoice" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('invoice', 'invoice_code_text')">
                            <vc-input v-model="invoice.code" :placeholder="tl('invoice', 'invoice_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('invoice', 'full_name_text')">
                            <vc-input v-model="invoice.full_name" :placeholder="tl('invoice', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('invoice', 'gender_text')">
                            <el-select v-model="invoice.gender" :placeholder="tl('invoice', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="invoice_name" :label="tl('invoice', 'invoice_name_text')">
                        <vc-input v-model="invoice.invoice_name" :placeholder="tl('invoice', 'invoice_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('invoice', 'password_text')">
                            <vc-input v-model="invoice.hashpass" type="password" show-password minlength="6" :placeholder="tl('invoice', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('invoice', 'email_text')">
                        <vc-input v-model="invoice.email" :placeholder="tl('invoice', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('invoice', 'phone_text')">
                        <vc-input v-model="invoice.phone" :placeholder="tl('invoice', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(invoiceForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import invoiceService from "@app/services/core/invoice.service";

    const rules= reactive({
        full_name: [
            { label: tl("invoice", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('invoice', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        invoice_name: [
            { label: tl("invoice", "invoice_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('invoice', 'invoice_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("invoice", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('invoice', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('invoice', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("invoice", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("invoice", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("invoice", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('invoice', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("invoice", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('invoice', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const invoiceForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const invoice = reactive({
    id: '',
    invoice_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (invoice.id) await getinvoiceDetail();
    });
    const getinvoiceDetail = async () => {
        const response = await invoiceService.detail(invoice.id);
        Object.assign(invoice, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (invoice.id) {
                await invoiceService.update(invoice).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await invoiceService.create(invoice).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let invoiceInfo = {
            id: '',
            invoice_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            invoiceInfo = (await invoiceService.detail(item))
        console.log(invoiceInfo)
        callback = _callback;
        Object.assign(invoice, invoiceInfo)
        modal.value.open();
    };

    const close = () => {
        if (invoiceForm.value) {
            invoiceForm.value.resetFields();
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