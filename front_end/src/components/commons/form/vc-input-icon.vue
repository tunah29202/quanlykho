<template>
    <el-input class="w-50 m-2"
              size="large"
              :prefix-icon="icon" />
</template>

<script lang="ts" setup>
defineProps<{
  icon: any;
}>();
</script>

<style lang="scss">
    @import "@/assets/styles/commons/vc-input";
</style>
