<template>
    <div class="vc-table">
        <el-table stripe style="width:100%" :height="height ?? '400px'" :data="datas" border v-loading="loading"
                  @selection-change="onRowSelected" @row-dblclick="onRowDbClick">
            <!-- EXPAND -->
            <el-table-column type="expand" v-if="tableConfig.expand">
                <template #default="scope">
                  <slot name="detail" :data="scope.row.carton_details" :scope="scope"></slot>
                </template>
            </el-table-column>
                  
            <!-- INDEX -->
            <el-table-column label="#" width="50" type="index" align="center" header-align="center"
              v-if="tableConfig.index" />

            <!-- CHECK BOX -->
            <el-table-column type="selection" width="40" v-if="tableConfig.checkbox" />

            <!-- DATA -->
            <template v-for="(col, index) in colConfigs" :key="index">
                <el-table-column :prop="col.key" :width="col.width" :label="col.title">
                    <template #default="scope">
                        <template v-if="col.key === 'imageUrls'">
                            <div class="flex space-x-2">
                                <img v-for="(url, imgIndex) in scope.row[col.key]" :key="imgIndex" :src="getImageUrl(url)" alt="" class="w-6 h-6" />
                            </div>
                        </template>
                        <template v-else>
                            {{ scope.row[col.key] }}
                        </template>
                    </template>
                </el-table-column>
            </template>
            <el-table-column v-if="tableConfig.action" :width="150" label="Tuỳ chỉnh" >
                <template #default="scope">
                    <div class="d-flex flex-center btn-group-action">
                        <slot name="action" :data="scope.row" :scope="scope" class="btn-action"></slot>
                    </div>
                </template>
            </el-table-column>
        </el-table>
        <div class=" table-footer pa-2, pt-3" v-if="tableConfig?.showPaging">
            <vc-pagination :pageConfig="pageConfig" @changed="onPageChanged"></vc-pagination>
        </div>

    </div>
</template>

    <script setup lang="ts">
    import { ref, toRefs, onMounted } from 'vue'
    import type { MetaResponse } from '@/interfaces/response.interface'
    import type { ColConfig, TableConfig } from '@/interfaces/table.interface'
    import VcPagination from './vc-pagination.vue'

    const props = defineProps<{
      datas?: any[]
      colConfigs: ColConfig[]
      tableConfig: TableConfig
      page: MetaResponse
      height?: string
      loading?: boolean
    }>()

    const {
      datas,
      tableConfig,
      colConfigs,
      page: pageConfig,
      height,
      loading,
    } = toRefs(props)

    const emit = defineEmits([
      'dbClick',
      'onDelete',
      'sorted',
      'rowSelected',
      'pageChanged',
    ])

    const colSettings = ref<ColConfig[]>([])

    onMounted(() => {
      colSettings.value = colConfigs.value
    })


    const getImageUrl = (url) => {
       return `http://localhost:7070/Images/${url}`;
    };
    
    const onPageChanged = (page: any) => {
      emit('pageChanged', page)
    }

    const onRowDbClick = (item: any) => {
      if (tableConfig.value.dbClick) emit('dbClick', item)
    }

    const onRowSelected = (items: any[]) => {
      emit('rowSelected', items)
    }

    /**
     * Event click sort table header
     * Emit sorted
     */
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const onSortChange = (config: any) => {
      if (config.column == null) {
        emit('sorted', null)
        return
      }

      emit(
        'sorted',
        `${config.prop}.${config.order == 'ascending' ? 'asc' : 'desc'}`
      )
    }
</script>
<style lang="scss">
    @import '@/assets/styles/commons/vc-table';
</style>
