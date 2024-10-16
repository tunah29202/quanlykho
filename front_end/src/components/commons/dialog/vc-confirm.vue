<template>
  <div class="vc-dialog">
    <el-dialog v-model="dialog" :title="title" :before-close="handleClose">
      <p style="padding: 12px 16px" v-html="message"></p>
      <template #footer>
        <span class="dialog-footer">
          <vc-button @click="onConfirm" v-if="!isConfirm" type="primary">
            {{ tl("Common", "BtnOk") }}
          </vc-button>
          <vc-button class="ml-2" @click="onCancel" v-if="isConfirm">
            {{ tl("Common", "btn_exit") }}
          </vc-button>
          <vc-button @click="onConfirm" v-if="isConfirm" type="danger">
            {{ tl("Common", "btn_delete") }}
          </vc-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import tl from "@/utils/locallize";


const emit = defineEmits(["confirmed", "canceled"]);

const dialog = ref(false);
const title = ref("");
const message = ref("");
const data = ref<any>(null);
const isConfirm = ref<boolean>(false);
let callback = (value: any) => {return value};

const confirm = (_title: any, _message: any, _callback: any) => {
  title.value = _title;
  message.value = _message;

  dialog.value = true;
  isConfirm.value = true;
  callback = _callback;
};
const onConfirm = () => {
  emit("confirmed", data.value);
  callback(true);
  dismissModal();
};

const onCancel = () => {
  emit("canceled");
  callback(false);
  dismissModal();
};

const handleClose = () => {
  onCancel();
};

const dismissModal = () => {
  dialog.value = false;
};

defineExpose({
  confirm,
});
</script>

<style lang="scss" scoped>
@import "@/assets/styles/commons/vc-confirm.scss";
</style>
