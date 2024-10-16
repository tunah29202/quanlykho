<template>
  <el-row class="vc-pagination">
    <el-col :span="24">
      <el-pagination size="small" background v-model:current-page="meta.page" v-model:page-size="meta.size"
        :page-sizes="[10, 15, 30, 50]" :total="pageConfig.total" @size-change="onSizeChange" @current-change="onPageChange"
        layout="total, sizes, prev, pager, next, jumper">
      </el-pagination>
    </el-col>
  </el-row>
</template>

<script setup lang="ts">
import type { MetaResponse } from "@/interfaces/response.interface";
import { toRef, ref, watch } from "vue";

const props = defineProps<{
  pageConfig: MetaResponse;
}>();

const meta = toRef(props, "pageConfig");
const startPage = ref(0);
const endPage = ref(0);
const totalItem = ref(0);
const totalPage = ref(0);

watch(meta, (newVal) => {
  calculate(newVal)
});

watch(props.pageConfig, (newVal) => {
  calculate(newVal)
});

const calculate = (newVal: any) => {
  const page = newVal.page ?? 1;
  const size = newVal.size ?? 10;
  const total = newVal.total ?? 0;
  startPage.value = size * (page - 1) + 1;
  endPage.value = total < size * page ? total : size * page;
  totalPage.value = Math.ceil(total / (size ?? 1));
  totalItem.value = total;
}

const emit = defineEmits(["changed"]);

const onPageChange = () => {
  emit("changed", meta.value);
};

const onSizeChange = () => {
  meta.value.page = 1;
  emit("changed", meta.value);
};
</script>

<style lang="scss">
@import "@/assets/styles/commons/vc-pagination.scss";
</style>
