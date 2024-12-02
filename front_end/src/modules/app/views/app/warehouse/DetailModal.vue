<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" bwarehouse v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Warehouse", "warehouse_code_text") }}
                        </div>
                    </template>
                    {{ warehouse.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Warehouse", "name_text") }}
                        </div>
                    </template>
                    {{ warehouse.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Warehouse", "address_text") }}
                    </div>
                </template>
                {{ warehouse.address }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Warehouse", "tel_text") }}
                    </div>
                </template>
                {{ warehouse.tel }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="warehouseForm" :model="warehouse" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('Warehouse', 'warehouse_code_text')">
                            <vc-input v-model="warehouse.code" :placeholder="tl('Warehouse', 'warehouse_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Warehouse', 'name_text')">
                            <vc-input v-model="warehouse.name" :placeholder="tl('Warehouse', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="address" :label="tl('Warehouse', 'address_text')">
                        <vc-input v-model="warehouse.address" :placeholder="tl('Warehouse', 'address_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="tel" :label="tl('Warehouse', 'tel_text')">
                        <vc-input v-model="warehouse.tel" :placeholder="tl('Warehouse', 'tel_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(warehouseForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import warehouseService from "@app/services/app/warehouse.service";

    const rules= reactive({
        code: [
            { label: tl("Warehouse", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Warehouse', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Warehouse", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Warehouse', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        address: [
            { label: tl("Warehouse", "address_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Warehouse', 'address_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        tel: [
            { label: tl("Warehouse", "tel_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Warehouse', 'tel_text'), validator: validate.phoneNumberRule, trigger: ["change"]},
            { label: tl('Warehouse', 'tel_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const warehouseForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const warehouse = reactive({
        id: '',
        code: '',
        name: '',
        address: '',
        tel: '',
    });

    onBeforeMount(async () => {
        if (warehouse.id) await getwarehouseDetail();
    });
    const getwarehouseDetail = async () => {
        const response = await warehouseService.detail(warehouse.id);
        Object.assign(warehouse, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (warehouse.id) {
                await warehouseService.update(warehouse).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await warehouseService.create(warehouse).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let warehouseInfo = {
            id: '',
            code: null,
            name: null,
            address: null,
            tel: null,
        };
        modalTitle.value = title;
        if (item)
            warehouseInfo = (await warehouseService.detail(item))
        callback = _callback;
        Object.assign(warehouse, warehouseInfo)
        console.log(warehouse)
        modal.value.open();
    };

    const close = () => {
        if (warehouseForm.value) {
            warehouseForm.value.resetFields();
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