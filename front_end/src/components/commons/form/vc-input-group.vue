<template>
    <el-form-item :label="label">
        <slot></slot>
    </el-form-item>
</template>

<script setup lang="ts">
defineProps<{

  required?: boolean;
  label?: string;
}>();
</script>
