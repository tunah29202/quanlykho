<template>
    <div class="page-login flex-center">
        <vc-card class="card-login pa-10">
            <el-form ref="loginFormRef" label-position="top" :model="request" :rules="rules">
                <div class="logo-container">
                    <img src="@/assets/images/logo.jpg" style="cursor: pointer" width="80"
                         height="80" />
                </div>
                <p class="logo-text">{{ tl('Login', 'welcome_message') }}</p>
                <vc-input-group required prop="user_name">
                    <vc-input-icon v-model="request.user_name" :icon="Avatar" :placeholder="tl('Login', 'user_name')"
                                   autocomplete="off" />
                </vc-input-group>
                <vc-input-group class="content-pwd" required prop="password">
                    <vc-input-icon v-model="request.password" :icon="Lock" :placeholder="tl('Login', 'password')"
                                   type="password" :show-password="true" />
                </vc-input-group>
                <vc-row :gutter="12" class="mt-10">
                    <vc-col :span="24" :md="{span:12}">
                        <vc-input-group required prop="captchaText">
                            <vc-input v-model="request.captchaText" class="vc-input-cap" :placeholder="tl('Login', 'captcha')" type="text"></vc-input>
                        </vc-input-group>
                    </vc-col>
                    <vc-col :span="24" :md="{ span: 12 }">
                        <div class="captcha">
                            <div class="captcha-content">
                                <div class="np-captcha-container">
                                    <div class="np-captcha" v-if="captcha && captcha.length">
                                        <div v-for="(c, i) in captcha" :key="i" :style="{
                                              fontSize: getFontSize() + 'px',
                                              fontWeight: 800,
                                              transform: 'rotate(' + getRotationAngle() + 'deg)',
                                              color: getRandomColor(),
                                            }" class="np-captcha-character protected-text">
                                            {{ c }}
                                        </div>
                                    </div>
                                </div>
                                <div class="d-flex justify-content-center align-items-center">
                                    <img src="@/assets/images/sync.png" style="cursor: pointer" @click="createCaptcha()" width="18"
                                         height="18" />
                                </div>
                            </div>
                        </div>
                    </vc-col>
                </vc-row>
                <div class="container-btn-login">
                    <vc-button type="primary" @click="onLogin(loginFormRef)" :loading="isLoading" style="color:white">
                        {{tl('Login', 'btn_login') }}
                    </vc-button>
                </div>
                <p class="forgot-pass mt-2">
                    <a style="cursor: pointer;" @click="clickForgotPassword" >{{ tl('Login', 'forgot_password') }}?</a>
                </p>
            </el-form>
        </vc-card>
        <el-dialog v-model="isClose" :before-close="handleClose" :show-close="false" :lock-scroll="true"
        :title="infoResetPass? tl('Login', 'forgot_password') : (verification? tl('Login', 'Verification') :(resetPassword? tl('Login', 'ResetPassword'):''))"
        width="450" >
            <el-form ref="resetFormRef" :model="infoResetForm" :rules="rulesResetForm">
                <vc-input-group v-if="infoResetPass" required prop="username" :label="tl('Login', 'user_name')" >
                    <vc-input v-model="infoResetForm.username" :placeholder="tl('User', 'user_name_holder')" />
                </vc-input-group>
                <vc-input-group v-if="infoResetPass" required prop="email" :label="tl('User', 'email_text')" >
                    <vc-input v-model="infoResetForm.email" :placeholder="tl('User', 'email_holder')" />
                </vc-input-group>
                <vc-input-group v-if="verification" required prop="verification" :label="tl('Login', 'confirmationCodeMessage')" >
                    <vc-input v-model="infoResetForm.verification" :placeholder="tl('Login', 'Verification')" />
                </vc-input-group>
                <vc-input-group v-if="resetPassword" required prop="newPass" :label="tl('ChangePassword', 'new_password')" >
                    <vc-input v-model="infoResetForm.newPass" :placeholder="tl('ChangePassword', 'new_password')"
                    type="password" show-password autocomplete="off" />
                </vc-input-group>
                <vc-input-group v-if="resetPassword" required prop="confirmPass" :label="tl('ChangePassword', 'confirm_password')" >
                    <vc-input v-model="infoResetForm.confirmPass" :placeholder="tl('ChangePassword', 'confirm_password')"
                    type="password" show-password autocomplete="off" />
                </vc-input-group>
            </el-form>
            <template #footer>
                <div class="dialog-footer" >
                    <el-button :loading="isLoadingResetPassword" @click="onCancel" >{{ tl('Common', 'btn_cancel') }}</el-button>
                    <el-button :loading="isLoadingResetPassword" style="color: white" type="primary" @click="onConfirm(resetFormRef)" >{{ tl('Common', 'btn_comfirm') }}</el-button>
                </div>
            </template>
        </el-dialog>
    </div>
</template>
<script setup lang="ts">
    import { reactive, onMounted, ref } from 'vue'
    import type { FormInstance } from 'element-plus'
    import { Avatar, Lock } from '@element-plus/icons-vue';
    import validate from '@/utils/validate'
    import tl from '@/utils/locallize'
    import { useAuthStore } from '@app/stores/core/auth.store';
    import { useRouter } from 'vue-router'; 
    import { storeToRefs } from 'pinia';
    import userService from '@app/services/core/user.service';
    import authService from '@app/services/core/auth.service';
    import useToast from '@/components/commons/alert/vc-toast.vue';

    const authStore = useAuthStore()
    const { loggedIn } = storeToRefs(authStore)
    const router = useRouter()
    const isLoading = ref<boolean>(false)
    const captcha = ref()
    const captchaText = ref('')
    const loginFormRef = ref<FormInstance>()
    const resetFormRef = ref<FormInstance>()
    const isClose = ref(false)
    const isLoadingResetPassword = ref<boolean>(false)
    const infoResetPass = ref(false)
    const verification = ref(false)
    const resetPassword = ref(false)


    const request = reactive({
        user_name: null,
        password: null,
        remember: true,
        captchaText: '',
    })

    const infoResetForm = reactive({
        user_id: '',
        username: '',
        email: '',
        verification: '',
        newPass: '',
        confirmPass: '',
    })
    const validateCapcha = (rule: any, value: any, callback: any) => {
        if (value.toUpperCase() === captchaText.value.toUpperCase()) {
            callback()
        } else {
            callback(new Error('Mã xác nhận không chính xác.'))
        }
    }

    const rules = reactive({
        user_name: [
            {required: true, validator: validate.required, trigger: 'blur', label: tl('Login', 'user_name')},
        ],
        password: [
            {required: true, validator: validate.required, trigger: 'blur', label: tl('Login', 'Password')},
        ],
        captchaText: [
            {required: true, validator: validate.required, trigger: 'blur', label: tl('Login', 'Capcha') },
            {validator: validateCapcha, trigger: 'blur'},
        ],
    })
    const clickForgotPassword =()=>{
        isClose.value = true;
        infoResetPass.value = true;
    }
    const handleClose = () => {
    if (isClose.value) close()
    }
    onMounted(() => {
        createCaptcha();
        if (loggedIn.value) {
            authStore.refresh().then((isLoggedIn) => {
            if (isLoggedIn) {
                router.push({
                name: 'Login',
                })
            }
            })
        }
    })
    const createCaptcha = () => {
        let tempCaptcha = ''
        for (let i = 0; i < 5; i++) {
            tempCaptcha += getRandomCharacter()
        }
        captchaText.value = tempCaptcha
        captcha.value = tempCaptcha.split('')
    }
    const onLogin = async (formEl: FormInstance | undefined) => {
        if (!formEl) return

        await formEl.validate(async (valid) => {
            if (!valid) return
            isLoading.value = true
            await authStore
                .login(request)
                .then((isLogged) => {
                    if (isLogged)
                    router.push({
                        name: 'Dashboard',
                    })
                })
                .finally(() => {
                    isLoading.value = false
                })
        })
    }
    const checkSamePassword = (rule: any, value: any, callback: any) => {
        if (value !== infoResetForm.newPass) {
            callback(
            new Error(tl("Login", "passwordMismatchMessage"))
            )
        } else {
            callback()
        }
    }
    const rulesResetForm = reactive({
    username: [
        {required: true, validator: validate.required, trigger: 'blur', label: tl('Login', 'user_name')},
    ],
    email: [
            { label: tl("User", "email_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl("User", "email_text"), validator: validate.emailRule, trigger: ["change"] },
            { label: tl('User', 'email_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
    verification: [
        {required: true, validator: validate.required, trigger: 'blur', label: tl('Login', 'verification'),},
    ],
    confirmPass: [
        {required: true, validator: validate.required, trigger: 'blur', label: tl('ChangePassword', 'confirm_password'),},
        {validator: checkSamePassword, trigger: 'blur' }, 
        { label: tl('User', 'password_text'), validator: validate.validatePassword, trigger: ["blur"] },
        { label: tl('User', 'password_text'), validator: validate.validatePassword, trigger: ["blur"] },
    ],
    newPass: [
        {required: true, validator: validate.required, trigger: 'blur', label: tl('ChangePassword', 'new_password'),},
        {label: tl('User', 'password_text'), validator: validate.validatePassword, trigger: ["blur"]},
    ],
    })

    const onCancel = ()=>{
        infoResetPass.value = false
        verification.value = false
        resetPassword.value = false
        isClose.value = false
        if(resetFormRef.value){
            resetFormRef.value.resetFields();
        }
    }
    const onConfirm = async (formEl: FormInstance| undefined) => {
        if(!formEl) return;
        await formEl.validate(async (valid)=>{
            if(!valid) return;
            isLoadingResetPassword.value = true;
            if(infoResetPass.value == true){
                const response = await userService.getUserByNameEmail({
                    user_name: infoResetForm.username,
                    email: infoResetForm.email
                })
                if(response.data){
                    infoResetForm.user_id = response.data.id;
                    const responseSendMail = await authService.sendEmail({
                        id: infoResetForm.user_id,
                        email: response.data.email
                    })
                    if(responseSendMail.code=='0'){
                        infoResetPass.value = false;
                        verification.value = true;
                    }
                }
            }
            else if (verification.value == true){
                const response = await authService.checkCodePass({
                    id: infoResetForm.user_id,
                    code: infoResetForm.verification
                })
                if(response.code == '0'){
                    verification.value = false;
                    resetPassword.value = true;
                }
            }
            else if (resetPassword.value == true){
                const response = await authService.forgotPassword({
                    id: infoResetForm.user_id,
                    new_password: infoResetForm.newPass
                })
                if(response.code =='0'){
                    await onCancel();
                    useToast.push({
                        type: "success",
                        message:tl('ChangePassword', 'ChangeSuccess')
                    })
                }
            }
            isLoadingResetPassword.value = false;
        })
    }
    const getRandomCharacter = () => {
        const symbols = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
        const randomNumber = Math.floor(Math.random() * 26)
        return symbols[randomNumber]
    }

    const getFontSize = () => {
        const fontVariations = [22, 18, 18, 25, 20]
        return fontVariations[Math.floor(Math.random() * 5)]
    }

    const getRotationAngle = () => {
        const rotationVariations = [5, 10, 15, 20, -5, -10, -15, -20]
        return rotationVariations[Math.floor(Math.random() * 8)]
    }

    const getRandomColor = () => {
        const letters = '0123456789ABCDEF'
        let color = '#'
        for (let i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)]
        }
        return color
    }

</script>
<style lang="scss">
    @import "@/assets/styles/page/login";
    .el-form-item {
    display: block;
    }
</style>