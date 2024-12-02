<template>
    <vc-modal ref="modal" :title="modalTitle" width="35%" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("function", "function_code_text") }}
                        </div>
                    </template>
                    {{ funct.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("function", "name_text") }}
                        </div>
                    </template>
                    {{ funct.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("function", "url_text") }}
                    </div>
                </template>
                {{ funct.url }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("function", "method_text") }}
                    </div>
                </template>
                {{ funct.method }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("function", "parent_cd_text") }}
                    </div>
                </template>
                {{ funct.parent_cd ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="functionForm" :model="funct" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('function', 'function_code_text')">
                            <vc-input v-model="funct.code" :placeholder="tl('function', 'function_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('function', 'name_text')">
                            <vc-input v-model="funct.name" :placeholder="tl('function', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="url" :label="tl('function', 'url_text')">
                        <vc-input v-model="funct.url" :placeholder="tl('function', 'url_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="method" :label="tl('function', 'method_text')">
                        <vc-input v-model="funct.method" :placeholder="tl('function', 'method_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="parent_cd" :label="tl('function', 'parent_cd_text')">
                        <vc-input v-model="funct.parent_cd" :placeholder="tl('function', 'parent_cd_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(functionForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import functionService from "@app/services/core/function.service";

    const rules= reactive({
        code: [
            { label: tl("funct", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('funct', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("funct", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('funct', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        url: [{ label: tl("function", "url_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        method: [
            { label: tl("function", "method_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('function', 'method_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        parent_cd: [
            { label: tl('function', 'parent_cd_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const functionForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const funct = reactive({
        id: '',
        code: '',
        name: '',
        url: '',
        method: '',
        parent_cd: '',
    });

    onBeforeMount(async () => {
        if (funct.id) await getfunctDetail();
    });
    const getfunctDetail = async () => {
        const response = await functionService.detail(funct.id);
        Object.assign(funct, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (funct.id) {
                await functionService.update(funct).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await functionService.create(funct).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let functInfo = {
            id: '',
            code: null,
            name: null,
            url: null,
            method: null,
            parent_cd: null,
        };
        modalTitle.value = title;
        if (item)
            functInfo = (await functionService.detail(item))
        callback = _callback;
        Object.assign(funct, functInfo)
        console.log(funct)
        modal.value.open();
    };

    const close = () => {
        if (functionForm.value) {
            functionForm.value.resetFields();
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