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
            </el-form>
        </vc-card>
    </div>
</template>
<script setup lang="ts">
    import { reactive, onMounted, ref } from 'vue'
    import type { FormInstance } from 'element-plus'
    import { Avatar, Lock } from '@element-plus/icons-vue';
    import validate from '@/utils/validate'
    import tl from '@/utils/locallize'
    import { useAuthStore } from '@app/stores/auth.store';
    import { useRouter } from 'vue-router'; 
    import { storeToRefs } from 'pinia';

    const authStore = useAuthStore()
    const { loggedIn } = storeToRefs(authStore)
    const router = useRouter()
    const isLoading = ref<boolean>(false)
    const captcha = ref()
    const captchaText = ref('')
    const loginFormRef = ref<FormInstance>()

    const request = reactive({
        user_name: null,
        password: null,
        remember: true,
        captchaText: '',
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
            {
            required: true,
            validator: validate.required,
            trigger: 'blur',
            label: tl('Login', 'user_name'),
            },
        ],
        password: [
            {
            required: true,
            validator: validate.required,
            trigger: 'blur',
            label: tl('Login', 'Password'),
            },
        ],
        captchaText: [
            {
            required: true,
            validator: validate.required,
            trigger: 'blur',
            label: tl('Login', 'Capcha'),
            },
            {
            validator: validateCapcha,
            trigger: 'blur',
            },
        ],
        })


    onMounted(() => {
        createCaptcha();

        if (loggedIn.value) {
            authStore.refresh().then((isLoggedIn) => {
                console.log(isLoggedIn)
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
                        name: 'UserList',
                    })
                })
                .finally(() => {
                    isLoading.value = false
                })
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
<style lang="scss" scoped>
    @import "@/assets/styles/page/login";
</style>