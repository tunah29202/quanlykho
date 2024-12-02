<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" binvoice v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Invoice", "invoice_code_text") }}
                        </div>
                    </template>
                    {{ invoice.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Invoice", "name_text") }}
                        </div>
                    </template>
                    {{ invoice.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Invoice", "url_text") }}
                    </div>
                </template>
                {{ invoice.url }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Invoice", "method_text") }}
                    </div>
                </template>
                {{ invoice.method }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Invoice", "parent_cd_text") }}
                    </div>
                </template>
                {{ invoice.parent_cd ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="invoiceForm" :model="invoice" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('Invoice', 'invoice_code_text')">
                            <vc-input v-model="invoice.code" :placeholder="tl('Invoice', 'invoice_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Invoice', 'name_text')">
                            <vc-input v-model="invoice.name" :placeholder="tl('Invoice', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="url" :label="tl('Invoice', 'url_text')">
                        <vc-input v-model="invoice.url" :placeholder="tl('Invoice', 'url_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="method" :label="tl('Invoice', 'method_text')">
                        <vc-input v-model="invoice.method" :placeholder="tl('Invoice', 'method_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="parent_cd" :label="tl('Invoice', 'parent_cd_text')">
                        <vc-input v-model="invoice.parent_cd" :placeholder="tl('Invoice', 'parent_cd_holder')" />
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
    import invoiceService from "@app/services/app/invoice.service";

    const rules= reactive({
        code: [
            { label: tl("Invoice", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Invoice', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Invoice", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Invoice', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        url: [{ label: tl("Invoice", "url_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        method: [
            { label: tl("Invoice", "method_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Invoice', 'method_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        parent_cd: [
            { label: tl('Invoice', 'parent_cd_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
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
        code: '',
        name: '',
        url: '',
        method: '',
        parent_cd: '',
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
            code: null,
            name: null,
            url: null,
            method: null,
            parent_cd: null,
        };
        modalTitle.value = title;
        if (item)
            invoiceInfo = (await invoiceService.detail(item))
        callback = _callback;
        Object.assign(invoice, invoiceInfo)
        console.log(invoice)
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