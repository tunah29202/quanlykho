<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Shipper", "shipper_name_text") }}
                        </div>
                    </template>
                    {{ shipper.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Shipper", "address_text") }}
                        </div>
                    </template>
                    {{ shipper.address ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Shipper", "fax_text") }}
                    </div>
                </template>
                {{ shipper.fax }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Shipper", "email_text") }}
                    </div>
                </template>
                {{ shipper.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Shipper", "tel_text") }}
                    </div>
                </template>
                {{ shipper.tel ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="shipperForm" :model="shipper" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="name" :label="tl('Shipper', 'shipper_name_text')">
                            <vc-input v-model="shipper.name" :placeholder="tl('Shipper', 'shipper_name_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="address" :label="tl('Shipper', 'address_text')">
                            <vc-input v-model="shipper.address" :placeholder="tl('Shipper', 'address_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="fax" :label="tl('Shipper', 'fax_text')">
                        <vc-input v-model="shipper.fax" :placeholder="tl('Shipper', 'fax_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('Shipper', 'email_text')">
                        <vc-input v-model="shipper.email" :placeholder="tl('Shipper', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="tel" :label="tl('Shipper', 'tel_text')">
                        <vc-input v-model="shipper.tel" :placeholder="tl('Shipper', 'tel_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" name="F00004"
                @click="onSave(shipperForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import shipperService from "@app/services/app/shipper.service";

    const rules= reactive({
        name: [
            { label: tl("Shipper", "shipper_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Shipper', 'shipper_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        address: [
            { label: tl("Shipper", "address_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Shipper', 'address_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 300 },
        ],
        fax: [{ label: tl("Shipper", "fax_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        email: [
            { label: tl("Shipper", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("Shipper", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('Shipper', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        tel: [
            { label: tl("Shipper", "tel_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Shipper', 'tel_text'), validator: validate.phoneNumberRule, trigger: ["change"]},
            { label: tl('Shipper', 'tel_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const shipperForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const shipper = reactive({
        id: '',
        name: '',
        address: '',
        fax: '',
        email: '',
        tel: '',
    });

    onBeforeMount(async () => {
        if (shipper.id) await getshipperDetail();
    });
    const getshipperDetail = async () => {
        const response = await shipperService.detail(shipper.id);
        Object.assign(shipper, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (shipper.id) {
                await shipperService.update(shipper).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await shipperService.create(shipper).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let shipperInfo = {
            id: '',
            name: null,
            address: null,
            fax: null,
            email: null,
            tel: null,
        };
        modalTitle.value = title;
        if (item)
            shipperInfo = (await shipperService.detail(item))
        callback = _callback;
        Object.assign(shipper, shipperInfo)
        modal.value.open();
    };

    const close = () => {
        if (shipperForm.value) {
            shipperForm.value.resetFields();
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