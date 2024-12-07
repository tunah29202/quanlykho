<template>
    <vc-modal ref="modal" :title="modalTitle" width="30%" :type="props.type" @close="close">
        <template #content>
            <el-descriptions style="padding: 12px 16px" :column="1" bcategory v-if="type ==POPUP_TYPE.VIEW">
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Category", "name_text") }}
                        </div>
                    </template>
                    {{ category.name ?? '-' }}
                </el-descriptions-item>
                <el-descriptions-item>
                    <template #label>
                        <div class="cell-item">
                            {{  tl("Category", "ingredient_id_text") }}
                        </div>
                    </template>
                    {{ category.ingredients.length === 0 ?'-' : '' }}
                    <template v-for="(item, index) in category.ingredients" :key="index">
                        {{ item.name }}
                        <span v-if="index !== category.ingredients.length -1">, </span>
                    </template>
                </el-descriptions-item>
            </el-descriptions>

            <el-form ref="categoryForm" :model="category" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right" v-else>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Category', 'name_text')">
                            <vc-input v-model="category.name" :placeholder="tl('Category', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="ingredients" :label="tl('Category', 'ingredient_id_text')">
                            <vc-input v-for="(ingredient, index) in category.ingredient_names" :key="index"
                             v-model="ingredient.name" :placeholder="tl('Category', 'ingredient_id_holder')" class="mb-2"/>
                            <el-button size="small" :icon="Plus" @click="addInput" style="width: 100%;">
                                {{ tl('Category', 'btn_add_ingredient') }}
                            </el-button> 
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
        </template>
        <template #acttion>
            <vc-button v-if="props.type != POPUP_TYPE.VIEW" type="primary" class="ml-2" code="F00004"
                @click="onSave(categoryForm)" :loading="isLoading" :icon="'FolderChecked'">
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
    import categoryService from "@app/services/app/category.service";
    import { Plus } from '@element-plus/icons-vue';

    const rules= reactive({
        name: [
            { label: tl("Category", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Category', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const categoryForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);

    let callback = (value: any) => { return value };

    const category = reactive({
        id: '',
        name: null,
        ingredient_names: [],
        ingredients: [],
    });
    const addInput = () =>
    {
        category.ingredient_names.push({
            name: ''
        });
    }
    onBeforeMount(async () => {
        if (category.id) await getcategoryDetail();
    });
    const getcategoryDetail = async () => {
        const response = await categoryService.detail(category.id);
        Object.assign(category, response);
    };
    const onSave = async (formEl: FormInstance | undefined) => {

        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;

            isLoading.value = true;

            if (category.id) {
                console.log(category)

                await categoryService.update(category).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await categoryService.create(category).finally(() => {
                    isLoading.value = false;
            });
            }
            callback(true);
            close();
        });
    };

    const open = async (title: any, item: any, _callback: any) => {
        let categoryInfo = {
            id: '',
            name: null,
            ingredient_names: [],
            ingredients: [],
        };
        modalTitle.value = title;
        if (item)
        {
            categoryInfo = (await categoryService.detail(item))
            for(let i = 0; i < categoryInfo.ingredients.length; i++)
            {
                const ingredient = categoryInfo.ingredients[i];
                category.ingredient_names.push({name: ingredient.name});
            }
        }
        callback = _callback;
        Object.assign(category, categoryInfo)
        modal.value.open();
    };

    const close = () => {
        if (categoryForm.value) {
            categoryForm.value.resetFields();
        }
        modal.value.close()
        category.ingredient_names = [];
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