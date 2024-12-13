<template>
    <div class="vc-page page-user">
        <vc-card>
            <h1 class="pb-4" style="border-bottom: 1px solid #cdcdcd">
                {{ tl('ChangePassword', 'Title') }}
            </h1>
            <vc-row class="mt-4" style="justify-content: center" >
                <vc-col :lg="8" :md="12" :sm="24" :xs="24" >
                    <el-form ref="changeForm" :model="changeFormInfo" :rules="rules" :label-position="'top'" > 
                        <vc-input-group requird prop="oldPass" :label="tl('ChangePassword', 'current_password')">
                            <vc-input v-model="changeFormInfo.oldPass" :placeholder="tl('ChangePassword', 'current_password')"
                            type="password" show-password autocomplete="off" />
                        </vc-input-group>
                        <vc-input-group requird prop="newPass" :label="tl('ChangePassword', 'new_password')">
                            <vc-input v-model="changeFormInfo.newPass" :placeholder="tl('ChangePassword', 'new_password')"
                            type="password" show-password autocomplete="off" />
                        </vc-input-group>
                        <vc-input-group requird prop="confirmPass" :label="tl('ChangePassword', 'confirm_password')">
                            <vc-input v-model="changeFormInfo.confirmPass" :placeholder="tl('ChangePassword', 'confirm_password')"
                            type="password" show-password autocomplete="off" />
                        </vc-input-group>
                        <vc-button type="primary" style="color: white;" @click="handleChangePassword(changeForm)" >{{ tl('ChangePassword', 'save_change') }}</vc-button>
                    </el-form>
                    
                </vc-col>
            </vc-row>
        </vc-card>
    </div>
</template>
<script setup lang="ts">
    import tl from '@/utils/locallize';
    import { onMounted, reactive, ref } from 'vue';
    import type { FormInstance } from 'element-plus';
    import validate from '@/utils/validate';
    import { useAuthStore } from '@app/stores/core/auth.store';
    import { storeToRefs } from 'pinia';
    import authService from '../../../services/core/auth.service';
    import { useRouter } from 'vue-router';

    const router = useRouter()
    const authStore = useAuthStore();
    const {account} = storeToRefs(authStore);
    const changeForm = ref<FormInstance>();
    
    const changeFormInfo = reactive({
        oldPass: '',
        newPass: '',
        confirmPass:''
    })
    const checkSamePassword = (rule: any, value: any, callback: any) => {
        if (value !== changeFormInfo.newPass) {
            callback(
            new Error(tl("Login", "passwordMismatchMessage"))
            )
        } else {
            callback()
        }
    }

    const rules = reactive({
        oldPass:[
            {required:true, validator: validate.required, trigger: 'blur', label: tl('ChangePassword', 'current_password')},
            {label: tl('User', 'password_text'), validator: validate.validatePassword, trigger:["blur"]}
        ],
        newPass:[
            {required:true, validator: validate.required, trigger: 'blur', label: tl('ChangePassword', 'new_password')},
            {label: tl('User', 'password_text'), validator: validate.validatePassword, trigger:["blur"]}
        ],
        confirmPass:[
            {required:true, validator: validate.required, trigger: 'blur', label: tl('ChangePassword', 'confirm_password')},
            {label: tl('User', 'password_text'), validator: validate.validatePassword, trigger:["blur"]},
            {validator: checkSamePassword, trigger: 'blur' }, 
        ],
    })

    const handleChangePassword = async (formEl: FormInstance | undefined)=>{
        if(!formEl) return;
        await formEl.validate((valid) => {
        if (valid) {
            onChangePassword()
        }
    })
    }
    const onChangePassword = async()=>{
        console.log(account)
        const data = {
            id: account.id,
            current_password: changeFormInfo.oldPass,
            new_password: changeFormInfo.newPass
        }
        if(data){
            await authService.changePassword(data)
            try{
                await authStore.logout();
                router.push({
                    name: 'Login'
                })
            }
            catch(error){
                console.error("Error")
            }
        }
    }
</script>
<style>
    .el-form-item {
    display: block;
    }
</style>