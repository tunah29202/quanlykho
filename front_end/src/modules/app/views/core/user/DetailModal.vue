<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("User", "user_code_text") }}
                        </div>
                    </template>
                    {{ user.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("User", "full_name_text") }}
                        </div>
                    </template>
                    {{ user.full_name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("User", "gender_text") }}
                    </div>
                </template>
                {{ user.gender }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("User", "email_text") }}
                    </div>
                </template>
                {{ user.email }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("User", "phone_text") }}
                    </div>
                </template>
                {{ user.phone ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="userForm" :model="user" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('User', 'user_code_text')">
                            <vc-input v-model="user.code" :placeholder="tl('User', 'user_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col :lg="12" :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="full_name" :label="tl('User', 'full_name_text')">
                            <vc-input v-model="user.full_name" :placeholder="tl('User', 'full_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                    <vc-col :lg=12 :md="12" :sm="24" :xs="24">
                        <vc-input-group required prop="gender" :label="tl('User', 'gender_text')">
                            <el-select v-model="user.gender" :placeholder="tl('User', 'gender_holder')">
                                <el-option :label="tl('Common', 'gender_male')" :value="tl('Common', 'gender_male')" />
                                <el-option :label="tl('Common', 'gender_female')" :value="tl('Common', 'gender_female')" />
                                <el-option :label="tl('Common', 'gender_other')" :value="tl('Common', 'gender_other')" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="user_name" :label="tl('User', 'user_name_text')">
                        <vc-input v-model="user.user_name" :placeholder="tl('User', 'user_name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20" v-if="type == POPUP_TYPE.CREATE">
                    <vc-col>
                        <vc-input-group required prop="hashpass" :label="tl('User', 'password_text')">
                            <vc-input v-model="user.hashpass" type="password" show-password minlength="6" :placeholder="tl('User', 'password_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="email" :label="tl('User', 'email_text')">
                        <vc-input v-model="user.email" :placeholder="tl('User', 'email_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="phone" :label="tl('User', 'phone_text')">
                        <vc-input v-model="user.phone" :placeholder="tl('User', 'phone_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="role_cd" :label="tl('User', 'role_text')" >
                            <el-select v-model="user.role_cd" >
                                <el-option v-for="item in dataGrid" :key="item.id" :label="item.name" :value="item.code" />
                            </el-select>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group :label="tl('User', 'warehouse_text')" >
                            <template v-for="(item, index) in user.warehouse_names" :key="index" >
                                <el-tag class="mr-2" type="info" disable-transitions >{{ item }}</el-tag>
                            </template>
                            <el-button size="small" :icon="Plus" @click="onAddWarehouse" >{{ tl('Common', 'btn_add_new') }}</el-button>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(userForm)" :loading="isLoading" :icon="'FolderChecked'">
                {{ tl("Common", props.type == POPUP_TYPE.CREATE ? "btn_add" : "btn_update") }}
            </vc-button>
        </template>
    </vc-modal>
    <vc-confirm ref="confirmDialog"></vc-confirm>
    <warehouse_modal ref="warehouseRef" @setWarehouse_ids="onSetWarehouse_ids"
    @setWarehouse_names="onSetWarehouse_names" >
    </warehouse_modal>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance } from "element-plus";
    import userService from "@app/services/core/user.service";
    import { useRoleStore } from '@app/stores/core/role.store';
    import { storeToRefs } from 'pinia';
    import WarehouseModal from './WarehouseModal.vue';
    import { Plus } from '@element-plus/icons-vue';    

    const storeRole = useRoleStore();
    const {dataGrid} = storeToRefs(storeRole);

    const rules= reactive({
        full_name: [
            { label: tl("User", "full_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('User', 'full_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
            ],
        user_name: [
            { label: tl("User", "user_name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('User', 'user_name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        hashpass: [
            { label: tl("User", "password_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('User', 'password_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 250 },
            { label: tl('User', 'password_text'), validator: validate.validatePassword, trigger: ["change"] },
        ],
        gender: [{ label: tl("User", "gender_text"), required: true, validator: validate.required, trigger: ["change"] }],
        email: [
            { label: tl("User", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("User", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('User', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        phone: [
            { label: tl("User", "phone_text"), validator: validate.phoneNumberRule, trigger: ["change"] },
            { label: tl('User', 'phone_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const userForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);
    const warehouseRef = ref<any>(null);

    let callback = (value: any) => { return value };

    const user = reactive({
    id: '',
    code: '',
    user_name: null,
    hashpass: null,
    full_name: null,
    email: null,
    phone: null,
    gender: '',
    role_cd: null,
    role_name: null,
    warehouse_ids: [],
    warehouse_names: [],
    });

    onBeforeMount(async () => {
        if (user.id) await getUserDetail();
        await storeRole.getList();
    });
    const getUserDetail = async () => {
        const response = await userService.detail(user.id);
        Object.assign(user, response);
    };
    const onAddWarehouse = ()=>{
        if (!warehouseRef.value) {
        console.error('warehouseRef chưa được khởi tạo!');
        return;
        }
        warehouseRef.value.open(user.id)
    }
    const onSetWarehouse_ids =(value: any)=>{
        user.warehouse_ids = value
    }
    const onSetWarehouse_names =(value:any)=>{
        user.warehouse_names = value
    }
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (user.id) {
                await userService.update(user).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await userService.create(user).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let userInfo = {      
            id: '',
            user_name: null,
            hashpass: null,
            is_actived: false,
            full_name: null,
            mail: null,
            phone: null,
            code: '',
            role_cd: '',
            gender: tl('Common', 'gender_male'),
            role_name: '',
            warehouse_ids: [],
            warehouse_names: []
        };
        modalTitle.value = title;
        if (item)
            userInfo = (await userService.detail(item))
        callback = _callback;
        Object.assign(user, userInfo)
        console.log(user)

        modal.value.open();
    };

    const close = () => {
        if (userForm.value) {
            userForm.value.resetFields();
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