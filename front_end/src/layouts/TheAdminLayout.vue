<template>
    <el-container class="v-application">
        <el-container>
            <TheSideBar :isCollapse="isCollapse"/>
            <el-main>
                <TheNavBar @toggleSidebar="onCollapse"/>
                <slot></slot>
            </el-main>
        </el-container>
    </el-container>
</template>
<script setup lang="ts">
    import TheSideBar from '@/components/layouts/TheSideBar.vue';
    import TheNavBar from '@/components/layouts/TheNavBar.vue'
    import { useAuthStore } from '../modules/app/stores/core/auth.store';
    import { onMounted, ref } from 'vue';
    import { useRouter } from 'vue-router';
    import { storeToRefs } from 'pinia';
    
    const authStore = useAuthStore()
    const {loggedIn} = storeToRefs(authStore)
    const router = useRouter();
    const isCollapse = ref<boolean>(false);
    const onCollapse = (value: any)=>{
        isCollapse.value = value;
    }
    
    onMounted(async ()=>{
        await authStore.refresh();
        if(!loggedIn.value){
            gotoLogin()
        }
    })
    const gotoLogin = ()=>{
        router.push({
            name: 'Login'
        })
    }

</script>
<style lang="scss">
    @import '@/assets/styles/main.scss';
</style>