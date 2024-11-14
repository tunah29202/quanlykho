<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("consignee", "consignee_code_text") }}
                        </div>
                    </template>
                    {{ consignee.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("consignee", "full_name_text") }}
                        </div>
                    </template>
                    {{ consignee.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("consignee", "gender_text") }}
                    </div>
                </template>
                {{ consignee.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("consignee", "email_text") }}
                    </div>
                </template>
                {{ consignee.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("consignee", "phone_text") }}
                    </div>
                </template>
                {{ consignee.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="consigneeForm" :model="consignee" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('consignee', 'consignee_code_text')">
                            <vc-input v-model="consignee.code" :placeholder="tl('consignee', 'consignee_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('consignee', 'full_name_text')">
                            <vc-input v-model="consignee.full_name" :placeholder="tl('consignee', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('consignee', 'gender_text')">
                            <el-select v-model="consignee.gender" :placeholder="tl('consignee', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="consignee_name" :label="tl('consignee', 'consignee_name_text')">
                        <vc-input v-model="consignee.consignee_name" :placeholder="tl('consignee', 'consignee_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('consignee', 'password_text')">
                            <vc-input v-model="consignee.hashpass" type="password" show-password minlength="6" :placeholder="tl('consignee', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('consignee', 'email_text')">
                        <vc-input v-model="consignee.email" :placeholder="tl('consignee', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('consignee', 'phone_text')">
                        <vc-input v-model="consignee.phone" :placeholder="tl('consignee', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(consigneeForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import consigneeService from "@app/services/core/consignee.service";

    const rules= reactive({
        full_name: [
            { label: tl("consignee", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('consignee', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        consignee_name: [
            { label: tl("consignee", "consignee_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('consignee', 'consignee_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("consignee", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('consignee', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('consignee', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("consignee", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("consignee", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("consignee", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('consignee', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("consignee", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('consignee', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const consigneeForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const consignee = reactive({
    id: '',
    consignee_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (consignee.id) await getconsigneeDetail();
    });
    const getconsigneeDetail = async () => {
        const response = await consigneeService.detail(consignee.id);
        Object.assign(consignee, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (consignee.id) {
                await consigneeService.update(consignee).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await consigneeService.create(consignee).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let consigneeInfo = {
            id: '',
            consignee_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            consigneeInfo = (await consigneeService.detail(item))
        console.log(consigneeInfo)
        callback = _callback;
        Object.assign(consignee, consigneeInfo)
        modal.value.open();
    };

    const close = () => {
        if (consigneeForm.value) {
            consigneeForm.value.resetFields();
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