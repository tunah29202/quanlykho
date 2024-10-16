<template>
  <el-dialog v-model="is_show" :show-close="false" :lock-scroll="true" :before-close="handleClose" align-center>
    <template #header="{ titleId, titleClass }">
      <div class="d-flex space-between align-center">
        <h4 :id="titleId" :class="titleClass">{{ title }}</h4>
      </div>
    </template>
    <template #footer="{ titleId, titleClass }">
      <div class="d-flex align-center" style="justify-content: end; padding: 0 16px 16px 0">
        <div>
          <slot name="acttion"></slot>
          <vc-button type="info" class="btn-close" @click="onClose" :icon="'Close'">
            {{ tl('Common', props.type == POPUP_TYPE.VIEW ? "btn_close" : "btn_cancel") }}
          </vc-button>
        </div>
      </div>
    </template>
    <slot name="content"></slot>
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, toRef } from 'vue'
import tl from '@/utils/locallize'
import { POPUP_TYPE } from "@/commons/const";

const props = defineProps<{
  title?: string,
  type: POPUP_TYPE;
}>()
const emit = defineEmits(['close'])

const is_show = ref(false)
const is_close = ref(false)
const title = toRef(props, 'title')

const handleClose = () => {
  if (is_close.value) close()
}

const onClose = () => {
  is_close.value = true
  handleClose()
  emit('close')
}

const open = () => {
  is_show.value = true
  is_close.value = false
}

const close = () => {
  is_show.value = false
  is_close.value = false
}

defineExpose({
  open,
  close,
})
</script>

<style lang="scss">
@import '@/assets/styles/commons/vc-modal.scss';
</style>
