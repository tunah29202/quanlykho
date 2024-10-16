<template>
    <div class="navbar pt-2 pb-2">
        <el-header class="navbar-header">
            <el-row :gutter="20">
                <el-col :span="2" style="padding: 0">
                    <el-header class="vertical-center">
                        <h1>Toggle</h1>
                    </el-header>
                </el-col>
                <el-col :span="20" class="col-right vertical-center">
                    <el-dropdown trigger="click">
                        <span class="el-dropdown-link">
                            <el-button :icon="UserFilled" type="info" size="large" circle />
                            <b class="ml-2">{{ account.full_name }}</b>
                        </span>
                        <template #dropdown>
                            <el-dropdown-menu>
                                <el-dropdown-item class="clearfix" >
                                    {{ tl("NavBar", "change_pass_text") }}
                                </el-dropdown-item>
                                <el-dropdown-item class="clearfix" @click="onLogout">
                                    {{ tl("NavBar", "logout_text") }}
                                </el-dropdown-item>
                            </el-dropdown-menu>
                        </template>
                    </el-dropdown>
                </el-col>
            </el-row>
        </el-header>
    </div>
</template>
<script setup lang="ts">
    import { useRouter } from 'vue-router'
    import { storeToRefs } from 'pinia'
    import { ref, onBeforeMount } from 'vue';
    import {
        Expand,
        Fold,
        UserFilled
    } from '@element-plus/icons-vue'
    import tl from '@/utils/locallize';
    import { useAuthStore } from '@app/stores/auth.store';

    const router = useRouter();
    const authStore = useAuthStore();
    const {account} = storeToRefs(authStore)

    const onLogout = async () => {
        try {
            await authStore.logout();
            router.push({ name: 'Login' });
        } catch (error) {
            console.error("Error");
        }
    }

    
</script>
<style lang="scss" scoped>
    @import '@/assets/styles/commons/navbar.scss';

    .vertical-center {
        display: flex;
        align-items: center;
    }

    @media (max-width: 575.98px) {
        .el-dropdown-link b {
            display: none;
        }
    }
</style>