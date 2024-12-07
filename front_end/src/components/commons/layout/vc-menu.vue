<template>
    <el-menu>
        <template v-for="(item, indexItem) in menu" :key="indexItem">
            <el-menu-item :index="`${indexItem}`" @click="goTo(item)" >
                <el-icon>
                    <component :is="item.icon"></component>
                </el-icon>
                <template #title>
                    {{ item.text }}
                </template>
            </el-menu-item>
        </template>
    </el-menu>
</template>
<script setup lang="ts">
    import { useRouter } from "vue-router";
    import menu from '@/commons/define/menu';
    import { storeToRefs } from 'pinia';
    import { useAuthStore } from '../../../modules/app/stores/core/auth.store';

    const authStore = useAuthStore();
    const {account} = storeToRefs(authStore);
    const router = useRouter();
    const goTo = (item: any) => {
        router.push({
            path: item.path ?? '/404'
        })
    }
</script>
<style>
</style>