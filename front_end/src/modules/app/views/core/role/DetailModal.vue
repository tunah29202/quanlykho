<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" brole v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Role", "role_code_text") }}
                        </div>
                    </template>
                    {{ role.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Role", "name_text") }}
                        </div>
                    </template>
                    {{ role.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Role", "url_text") }}
                    </div>
                </template>
                {{ role.url }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Role", "method_text") }}
                    </div>
                </template>
                {{ role.method }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Role", "parent_cd_text") }}
                    </div>
                </template>
                {{ role.parent_cd ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="roleForm" :model="role" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('Role', 'role_code_text')">
                            <vc-input v-model="role.code" :placeholder="tl('Role', 'role_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Role', 'name_text')">
                            <vc-input v-model="role.name" :placeholder="tl('Role', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="url" :label="tl('Role', 'url_text')">
                        <vc-input v-model="role.url" :placeholder="tl('Role', 'url_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="method" :label="tl('Role', 'method_text')">
                        <vc-input v-model="role.method" :placeholder="tl('Role', 'method_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="parent_cd" :label="tl('Role', 'parent_cd_text')">
                        <vc-input v-model="role.parent_cd" :placeholder="tl('Role', 'parent_cd_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(roleForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import roleService from "@app/services/core/role.service";

    const rules= reactive({
        code: [
            { label: tl("Role", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Role', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Role", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Role', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        url: [{ label: tl("Role", "url_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        method: [
            { label: tl("Role", "method_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Role', 'method_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        parent_cd: [
            { label: tl('Role', 'parent_cd_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const roleForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const role = reactive({
        id: '',
        code: '',
        name: '',
        url: '',
        method: '',
        parent_cd: '',
    });

    onBeforeMount(async () => {
        if (role.id) await getroleDetail();
    });
    const getroleDetail = async () => {
        const response = await roleService.detail(role.id);
        Object.assign(role, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (role.id) {
                await roleService.update(role).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await roleService.create(role).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let roleInfo = {
            id: '',
            code: null,
            name: null,
            url: null,
            method: null,
            parent_cd: null,
        };
        modalTitle.value = title;
        if (item)
            roleInfo = (await roleService.detail(item))
        callback = _callback;
        Object.assign(role, roleInfo)
        console.log(role)
        modal.value.open();
    };

    const close = () => {
        if (roleForm.value) {
            roleForm.value.resetFields();
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