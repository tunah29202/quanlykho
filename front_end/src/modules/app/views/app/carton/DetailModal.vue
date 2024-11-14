<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("carton", "carton_code_text") }}
                        </div>
                    </template>
                    {{ carton.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("carton", "full_name_text") }}
                        </div>
                    </template>
                    {{ carton.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("carton", "gender_text") }}
                    </div>
                </template>
                {{ carton.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("carton", "email_text") }}
                    </div>
                </template>
                {{ carton.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("carton", "phone_text") }}
                    </div>
                </template>
                {{ carton.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="cartonForm" :model="carton" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('carton', 'carton_code_text')">
                            <vc-input v-model="carton.code" :placeholder="tl('carton', 'carton_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('carton', 'full_name_text')">
                            <vc-input v-model="carton.full_name" :placeholder="tl('carton', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('carton', 'gender_text')">
                            <el-select v-model="carton.gender" :placeholder="tl('carton', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="carton_name" :label="tl('carton', 'carton_name_text')">
                        <vc-input v-model="carton.carton_name" :placeholder="tl('carton', 'carton_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('carton', 'password_text')">
                            <vc-input v-model="carton.hashpass" type="password" show-password minlength="6" :placeholder="tl('carton', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('carton', 'email_text')">
                        <vc-input v-model="carton.email" :placeholder="tl('carton', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('carton', 'phone_text')">
                        <vc-input v-model="carton.phone" :placeholder="tl('carton', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(cartonForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import cartonService from "@app/services/core/carton.service";

    const rules= reactive({
        full_name: [
            { label: tl("carton", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('carton', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        carton_name: [
            { label: tl("carton", "carton_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('carton', 'carton_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("carton", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('carton', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('carton', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("carton", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("carton", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("carton", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('carton', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("carton", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('carton', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const cartonForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const carton = reactive({
    id: '',
    carton_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    code: '',
    });

    onBeforeMount(async () => {
        if (carton.id) await getcartonDetail();
    });
    const getcartonDetail = async () => {
        const response = await cartonService.detail(carton.id);
        Object.assign(carton, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (carton.id) {
                await cartonService.update(carton).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await cartonService.create(carton).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let cartonInfo = {
            id: '',
            carton_name: null,
            hashpass: null,
            full_name: null,
            email: null,
            phone: null,
            code: null,
            gender: tl('Common', 'gender_male'),
        };
        modalTitle.value = title;
        if (item)
            cartonInfo = (await cartonService.detail(item))
        console.log(cartonInfo)
        callback = _callback;
        Object.assign(carton, cartonInfo)
        modal.value.open();
    };

    const close = () => {
        if (cartonForm.value) {
            cartonForm.value.resetFields();
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