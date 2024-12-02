<template>
    <vc-modal ref="modal" :title="modalTitle" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" border v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Order", "order_code_text") }}
                        </div>
                    </template>
                    {{ order.code ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Order", "name_text") }}
                        </div>
                    </template>
                    {{ order.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Order", "url_text") }}
                    </div>
                </template>
                {{ order.url }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Order", "method_text") }}
                    </div>
                </template>
                {{ order.method }}
                </el-descriptions-item>
                <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                    {{ tl("Order", "parent_cd_text") }}
                    </div>
                </template>
                {{ order.parent_cd ?? '-' }}
                </el-descriptions-item>
            </el-descriptions>

            <el-form  ref="orderForm" :model="order" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('Order', 'order_code_text')">
                            <vc-input v-model="order.code" :placeholder="tl('Order', 'order_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Order', 'name_text')">
                            <vc-input v-model="order.name" :placeholder="tl('Order', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="url" :label="tl('Order', 'url_text')">
                        <vc-input v-model="order.url" :placeholder="tl('Order', 'url_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="method" :label="tl('Order', 'method_text')">
                        <vc-input v-model="order.method" :placeholder="tl('Order', 'method_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group prop="parent_cd" :label="tl('Order', 'parent_cd_text')">
                        <vc-input v-model="order.parent_cd" :placeholder="tl('Order', 'parent_cd_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(orderForm)" :loading="isLoading" :icon="'FolderChecked'">
                {{ tl("Common", props.type == POPUP_TYPE.CREATE ? "btn_add" : "btn_update") }}
            </vc-button>
        </template>
        <vc-confirm ref="confirmDialog"></vc-confirm>
    </vc-modal>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance } from "element-plus";
    import orderService from "@app/services/app/order.service";

    const rules= reactive({
        code: [
            { label: tl("Order", "code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Order', 'code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Order", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Order', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        url: [{ label: tl("Order", "url_text"), required: true, validator: validate.required, trigger: ["blur"] }],
        method: [
            { label: tl("Order", "method_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Order', 'method_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        parent_cd: [
            { label: tl('Order', 'parent_cd_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 15 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const orderForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const order = reactive({
        id: '',
        code: '',
        name: '',
        url: '',
        method: '',
        parent_cd: '',
    });

    onBeforeMount(async () => {
        if (order.id) await getorderDetail();
    });
    const getorderDetail = async () => {
        const response = await orderService.detail(order.id);
        Object.assign(order, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (order.id) {
                await orderService.update(order).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await orderService.create(order).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let orderInfo = {
            id: '',
            code: null,
            name: null,
            url: null,
            method: null,
            parent_cd: null,
        };
        modalTitle.value = title;
        if (item)
            orderInfo = (await orderService.detail(item))
        callback = _callback;
        Object.assign(order, orderInfo)
        console.log(order)
        modal.value.open();
    };

    const close = () => {
        if (orderForm.value) {
            orderForm.value.resetFields();
        }
        modal.value.close()
    };

    defineExpose({
        open,
        close,
    });


</script>
<style>
    .el-form-item {
    display: block;
    }
</style>