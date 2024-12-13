<template>
    <el-menu>
        <template v-for="(item, indexItem) in filteredMenu" :key="indexItem">
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
    import { computed } from "vue";

    const authStore = useAuthStore();
    const {account, permissions} = storeToRefs(authStore);
    const router = useRouter();
    const filteredMenu = computed(() => {
        return menu.filter((item) => {
            return !item.permission || permissions.value.includes(item.permission);
        });
    });
    const goTo = (item: any) => {
        router.push({
            path: item.path ?? '/404'
        })
    }
</script>
<style>
</style>