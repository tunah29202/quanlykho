<template>
    <el-scrollbar height="100vh" class="side-bar">
        <div class="sidebar-header">
            <vc-row :gutter="20">
                <vc-col :span="6">
                    <img src="@/assets/images/logo.jpg" style="cursor: pointer;" width="40" height="40" />
                </vc-col>
                <vc-col :span="18">
                    <el-dropdown>
                        <span class="el-dropdown-link, ml-2" style="color: white; cursor: pointer;">
                            Chọn kho
                            <el-icon class="el-icon--right">
                                <arrow-down />
                            </el-icon>
                        </span>
                        <template #dropdown>
                            <el-scrollbar max-height="400px">
                                <el-dropdown-menu>
                                    <el-dropdown-item v-for="item in dataGridAll" :key="item.code" :comman="item">
                                        {{item.name}}
                                    </el-dropdown-item>
                                </el-dropdown-menu>
                            </el-scrollbar>
                        </template>
                    </el-dropdown>
                </vc-col>
            </vc-row>
            <hr />
        </div>
        <vc-menu ></vc-menu>
    </el-scrollbar>
</template>
<script setup lang="ts">
import { storeToRefs } from "pinia";
import { onMounted, ref, toRef } from "vue";
import { useRoute, useRouter } from "vue-router";

    const currentRoute = useRoute();
    const router = useRouter();
    const dataGridAll = ref([
        { code: "W1", name: "Warehouse 1" },
        { code: "W2", name: "Warehouse 2" },
        { code: "W3", name: "Warehouse 3" },
        { code: "W4", name: "Warehouse 4" },
    ]);
</script>
<style lang="scss">
    @import "@/assets/styles/commons/sidebar";
    .sidebar-header {
        padding: 20px 20px;
        color: white;
    }

    h1 {
        font-size: 2em;
        margin-bottom: 5px;
    }

    @media (min-width: 1024px) {
        .el-scrollbar {
            overflow: visible !important;
        }
    }
</style>