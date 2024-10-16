<template>
    <el-icon>
        <component v-bind:is="type"></component>
    </el-icon>
</template>

<script setup lang="ts">
defineProps<{
  type?: string;
}>();
</script>
