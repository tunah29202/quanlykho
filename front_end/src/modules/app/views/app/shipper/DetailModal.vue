<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Shipper", "shipper_code_text") }}
                        </div>
                    </template>
                    {{ shipper.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Shipper", "name_text") }}
                        </div>
                    </template>
                    {{ shipper.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Shipper", "url_text") }}
                    </div>
                </template>
                {{ shipper.url }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Shipper", "method_text") }}
                    </div>
                </template>
                {{ shipper.method }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Shipper", "parent_cd_text") }}
                    </div>
                </template>
                {{ shipper.parent_cd ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="shipperForm" :model="shipper" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('Shipper', 'shipper_code_text')">
                            <vc-input v-model="shipper.code" :placeholder="tl('Shipper', 'shipper_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Shipper', 'name_text')">
                            <vc-input v-model="shipper.name" :placeholder="tl('Shipper', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="url" :label="tl('Shipper', 'url_text')">
                        <vc-input v-model="shipper.url" :placeholder="tl('Shipper', 'url_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="method" :label="tl('Shipper', 'method_text')">
                        <vc-input v-model="shipper.method" :placeholder="tl('Shipper', 'method_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="parent_cd" :label="tl('Shipper', 'parent_cd_text')">
                        <vc-input v-model="shipper.parent_cd" :placeholder="tl('Shipper', 'parent_cd_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
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
        code: [
            { label: tl("Shipper", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Shipper', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Shipper", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Shipper', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        url: [{ label: tl("Shipper", "url_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        method: [
            { label: tl("Shipper", "method_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Shipper', 'method_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        parent_cd: [
            { label: tl('Shipper', 'parent_cd_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
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
        code: '',
        name: '',
        url: '',
        method: '',
        parent_cd: '',
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
            code: null,
            name: null,
            url: null,
            method: null,
            parent_cd: null,
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