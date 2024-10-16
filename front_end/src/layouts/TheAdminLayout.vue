<template>
    <el-container class="v-application">
        <el-container>
            <TheSideBar />
            <el-main>
                <TheNavBar />
                <slot></slot>
            </el-main>
        </el-container>
    </el-container>
</template>
<script setup lang="ts">
import TheSideBar from '@/components/layouts/TheSideBar.vue';
import TheNavBar from '@/components/layouts/TheNavBar.vue'

</script>
<style lang="scss">
    @import '@/assets/styles/main.scss';
    @import '@/assets/styles/page/client/_footerClient.scss';
</style>