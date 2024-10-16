
<template>
    <component :is="layout">
        <RouterView />
    </component>
</template>
<script lang="ts" setup>
import { computed, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const layout = computed(() => {
  const layout = route?.meta?.layout ?? "Admin";
  if (layout) {
    return `The${layout}Layout`;
  }
  return 'div';
});

</script>