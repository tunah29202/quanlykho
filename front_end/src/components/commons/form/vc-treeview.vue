<template>
    <el-tree ref="treeRef"
             :node-key=" node_key ?? 'value'"
             :data="data"
             :props="configs"
             @check="onSelected" />
</template>

<script setup lang="ts">
import { watch, ref, toRef } from "vue";
import { ElTree } from "element-plus";

const configs = {
  label: "name",
  children: "items",
};

const props = defineProps<{
  data?: [];
  node_key?: "";
  modelValue: any;
}>();

const emit = defineEmits(["update:modelValue"]);

const modelValue = toRef(props, "modelValue");
const data = toRef(props, "data");

watch(modelValue, (newVal) => {
  setCheckedKeys(newVal);
});

const treeRef = ref<InstanceType<typeof ElTree>>();

// const getCheckedNodes = () => {
//   return treeRef.value?.getCheckedNodes(false, false);
// };

const getCheckedKeys = () => {
  return treeRef.value?.getCheckedKeys(false);
};

const setCheckedKeys = (keys: any) => {
  treeRef.value?.setCheckedKeys(keys, false);
};

const onSelected = () => {
  emit("update:modelValue", getCheckedKeys());
};
</script>
