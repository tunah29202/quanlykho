<template>
    <div vc-page page-role>
        <vc-card >
            <el-breadcrumb :separator-icon="ArrowRight" class="pb-4" style="border-bottom: 1px solid #cdcdcd;" >
                <el-breadcrumb-item :to="{path: '/role'}">{{ tl('SideBar', 'role_management') }}</el-breadcrumb-item>
                <el-breadcrumb-item>{{ _id ? tl('Role', 'edit_text') :tl('Role', 'add_new_text')  }}</el-breadcrumb-item>
            </el-breadcrumb>
            <el-form  ref="roleForm" :model="role" :rules="rules" label-position="right" style="padding: 12px 16px"
            require-asterisk-position="right">
                <vc-row :gutter=20>
                    <vc-col>
                        <vc-input-group prop="code" :label="tl('Role', 'role_code_text')">
                            <vc-input v-model="role.code" :placeholder="tl('Role', 'role_code_holder')"/>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="name" :label="tl('Role', 'name_text')">
                            <vc-input v-model="role.name" :placeholder="tl('Role', 'name_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="description" :label="tl('Role', 'description_text')">
                            <vc-input type="textarea" row="3" v-model="role.description" :placeholder="tl('Role', 'description_holder')" />
                        </vc-input-group>
                    </vc-col>
                </vc-row>
                <vc-row :gutter="20">
                    <vc-col>
                        <vc-input-group required prop="function" :label="tl('User', 'role_text')" >
                            <el-table :data="funtions" border style="width: 100%" stripe
                            :header-cell-style="{ fontSize: '16px', backgroundColor: '#f5f7fa', color: '#606266' }">
                                <el-table-column prop="name" label="Chức năng" width="300" >
                                    <template #default="{ row }">
                                        {{ row.name }}
                                    </template>
                                </el-table-column>
                                <el-table-column :label="tl('Function', 'check_all_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <el-checkbox  v-model="row.selectAll" :indeterminate="row.isIndeterminate" @change="selectAllChanged(row)">
                                        </el-checkbox>
                                    </template>
                                </el-table-column>
                                <el-table-column :label="tl('Function', 'view_list_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'GET' && item.name.toLowerCase().includes('list') "
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
                                <el-table-column :label="tl('Function', 'view_detail_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'GET' && !item.name.toLowerCase().includes('list') && item.name != 'Export Invoice' && item.name != 'Export Product' "
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
    
                                <el-table-column :label="tl('Function', 'add_new_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'POST' && item.name != 'Import Invoice' && item.name != 'Import Product' "
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
    
                                <el-table-column :label="tl('Function', 'edit_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'PUT'"
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
    
                                <el-table-column :label="tl('Function', 'delete_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'DELETE'"
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
                                <el-table-column :label="tl('Function', 'export_text')" align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'GET' && item.name ==='Export Invoice' || item.name === 'Export Product' "
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
                                <el-table-column :label="tl('Function', 'import_text')"align="center" header-align="center" >
                                    <template #default="{ row }">
                                        <div v-for="item in row.items" :key="item.id">
                                            <el-checkbox
                                                v-if="item.method === 'POST' && item.name ==='Import Invoice' || item.name === 'Import Product' "
                                                v-model="item.checked"
                                                @change="updateParent(row)">
                                            </el-checkbox>
                                        </div>
                                    </template>
                                </el-table-column>
                            </el-table>
                        </vc-input-group>
                    </vc-col>
                </vc-row>
            </el-form>
            
            <div class="d-flex align-center" style="justify-content: end; padding: 16px" >
                <div>
                    <vc-button type="primary" :loading="isLoading" class="btn-close" @click=onSave(roleForm) >
                        {{  _id ? tl('Common', 'btn_update') : tl('Common', 'btn_add') }}
                    </vc-button>
                    <vc-button type="info" @click="onClickBack">
                            {{ tl('Common', 'btn_exit') }}
                    </vc-button>
                </div>
            </div>
        </vc-card>
    </div>
</template>
<script setup lang="ts">
    import { POPUP_TYPE } from '@/commons/const';
    import tl from '@/utils/locallize';
    import { onBeforeMount, ref, reactive } from "vue";
    import validate from "@/utils/validate";
    import type { FormInstance } from "element-plus";
    import roleService from "@app/services/core/role.service";
    import functionService from '@app/services/core/function.service';
    import { ArrowRight } from '@element-plus/icons-vue';
    import { useRoute, useRouter } from 'vue-router';

    const route = useRoute()
    const router = useRouter()
    const rules= reactive({
        code: [
            { label: tl("Role", "role_code_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Role', 'role_code_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 30 },
            ],
        name: [
            { label: tl("Role", "name_text"), required: true, validator: validate.required, trigger: ["blur"] },
            { label: tl('Role', 'name_text'), validator: validate.maxLengthRule, trigger: ["blur"], max: 100 },
        ],
        description: [{ label: tl("Role", "description_text"), required: true, validator: validate.required, trigger: ["blur"] }],
    });
    const props = defineProps<{
        type: POPUP_TYPE;
    }>();

    const roleForm = ref<FormInstance>();
    const isLoading = ref(false);
    const confirmDialog = ref<any>(null);
    const modal = ref<any>(null);
    const modalTitle = ref<any>(null);
    const funtions = ref<any>([]);
    let _id = route.params.id as string;

    

    let callback = (value: any) => { return value };

    const role = reactive({
        id: '',
        code: '',
        name: '',
        description: null,
        permissions: [],
    });
    const selectAll = ref(false);
    const isIndeterminate = ref(false);
    onBeforeMount(async () => {
        await getFunction()
        if (_id) {
            await getRoleDetail();
        }
    });
    const getRoleDetail = async () => {
        const response = await roleService.detail(_id);
        Object.assign(role, response);
        initializeCheckedState();
    };
    const initializeCheckedState = () => {
        funtions.value.forEach(row => {
            row.items.forEach(item => {
                item.checked = role.permissions.includes(item.code);
            });
            updateIndeterminate(row);
        });
    };
    const getFunction = async () =>{
        await functionService.getTreeView().then((response)=>{
            funtions.value = response.data.result;
        })
    }
    const selectAllChanged = (row) => {
        row.items.forEach(item => {
            item.checked = row.selectAll; 
        });
        updateIndeterminate(row);
        updatePermissions()
    };

    const updateIndeterminate = (row) => {
        const checkedCount = row.items.filter(item => item.checked).length;
        row.selectAll = checkedCount === row.items.length;
        row.isIndeterminate = checkedCount > 0 && checkedCount < row.items.length;
    };

    const updateParent = (row) => {
        const totalItems = row.items.length;
        const selectedCount = row.items.filter(item => item.checked).length;
        row.selectAll = selectedCount === totalItems;
        row.isIndeterminate = selectedCount > 0 && selectedCount < totalItems;
        updatePermissions();
    };

    const updatePermissions = () => {
        role.permissions = funtions.value.flatMap(row =>
        row.items.filter(item => item.checked).map(item => item.code)
    )};
    
    const onSave = async (formEl: FormInstance | undefined) => {
        if (!formEl) return;

        await formEl.validate(async (valid) => {
            if (!valid) return;
            isLoading.value = true;

            if (role.id) {
                await roleService.update(role).finally(() => {
                    isLoading.value = false;
            });
            } else {
                await roleService.create(role).finally(() => {
                    isLoading.value = false;
            });
            }
        });
        router.push({
            name: 'RoleList'
        })
    };
    const onClickBack = () =>{
        router.push({
            name: 'RoleList'
        })
    }
</script>
<style>
    .el-form-item {
    display: block;
    }
    .checkbox-label {
    font-weight: normal;  
    }
    .el-tree-node.is-expanded > .el-tree-node__children {
    display: flex !important;
    flex-wrap: wrap;
    gap: 10px;
    
    }

</style>