<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("package", "package_code_text") }}
                        </div>
                    </template>
                    {{ package.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("package", "full_name_text") }}
                        </div>
                    </template>
                    {{ package.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("package", "gender_text") }}
                    </div>
                </template>
                {{ package.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("package", "email_text") }}
                    </div>
                </template>
                {{ package.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("package", "phone_text") }}
                    </div>
                </template>
                {{ package.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="packageForm" :model="package" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('package', 'package_code_text')">
                            <vc-input v-model="package.code" :placeholder="tl('package', 'package_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('package', 'full_name_text')">
                            <vc-input v-model="package.full_name" :placeholder="tl('package', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('package', 'gender_text')">
                            <el-select v-model="package.gender" :placeholder="tl('package', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="package_name" :label="tl('package', 'package_name_text')">
                        <vc-input v-model="package.package_name" :placeholder="tl('package', 'package_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('package', 'password_text')">
                            <vc-input v-model="package.hashpass" type="password" show-password minlength="6" :placeholder="tl('package', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('package', 'email_text')">
                        <vc-input v-model="package.email" :placeholder="tl('package', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('package', 'phone_text')">
                        <vc-input v-model="package.phone" :placeholder="tl('package', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(packageForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import packageService from "@app/services/core/package.service";

    const rules= reactive({
        full_name: [
            { label: tl("package", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('package', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        package_name: [
            { label: tl("package", "package_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('package', 'package_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("package", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('package', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('package', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("package", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("package", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("package", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('package', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("package", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('package', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const packageForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const package = reactive({
    id: '',
    package_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (package.id) await getpackageDetail();
    });
    const getpackageDetail = async () => {
        const response = await packageService.detail(package.id);
        Object.assign(package, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (package.id) {
                await packageService.update(package).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await packageService.create(package).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let packageInfo = {
            id: '',
            package_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            packageInfo = (await packageService.detail(item))
        console.log(packageInfo)
        callback = _callback;
        Object.assign(package, packageInfo)
        modal.value.open();
    };

    const close = () => {
        if (packageForm.value) {
            packageForm.value.resetFields();
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