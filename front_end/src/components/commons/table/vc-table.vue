<template>
    <div class="vc-table">
        <el-table stripe style="width:100%" :height="height ?? '400px'" :data="datas" border v-loading="loading"
                  @selection-change="onRowSelected" @row-dblclick="onRowDbClick">
            <!-- EXPAND -->
            <el-table-column type="expand" v-if="tableConfig.expand">
              <template #default="scope">
                <template v-if="scope.row.carton_details && scope.row.carton_details.length > 0">
                  <slot name="detail" :data="scope.row.carton_details" :scope="scope"></slot>
                </template>
                <template v-else>
                  <slot name="detail" :data="scope.row.order_details" :scope="scope"></slot>
                </template>
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
                        <template v-if="col.key === 'image'">
                            <div class="flex space-x-2 flex-center">
                              <el-image style="width: 50px; height: 50px;" fit="cover" :src="getUrlImage(scope.row[col.key])"/>
                            </div>
                        </template>
                        <template v-else>
                          <template v-if="col.key === 'ingredients' ">
                            <div>
                              <template v-for="(item, index) in scope.row.ingredients" :key="index" >
                                {{ item.name }}
                                <span v-if="index !== scope.row.ingredients.length - 1">, </span>
                              </template>
                            </div>
                          </template>
                          <template v-else >
                            <template v-if="col.key === 'warehouses' ">
                              <div>
                                <template v-for="(item, index) in scope.row.warehouse_names" :key="index" >
                                  {{ item }}
                                  <span v-if="index !== scope.row.warehouse_names.length - 1">, </span>
                                </template>
                              </div>
                            </template>
                            <template v-else>
                              <template v-if="col.key === 'payment_method' ">
                                  {{ scope.row[col.key]?.name }}
                              </template>
                              <template v-else>
                                <template v-if="col.key === 'customer_company' ">
                                  {{ scope.row.order?.customer?.company_name }}
                                </template>
                                <template v-else>
                                  <template v-if="col.key === 'customer'">
                                    {{ scope.row?.[col.key]?.company_name }}
                                  </template>
                                  <template v-else >
                                    <template v-if="col.key === 'status' && tableConfig.invoice">
                                      <el-tag :type="scope.row.status == 0 ? 'info' : 'success'" disable-transitions>
                                        {{scope.row.status == 0
                                        ? tl("Invoice", "not_exported_text") : tl("Invoice", "exported_text") }}</el-tag>
                                    </template>
                                    <template v-else>
                                      <template v-if="col.key === 'status' && tableConfig.order" >
                                        <el-tag :type="getTagTypeStatus(scope.row.status)" disable-transitions>
                                        {{ getLabelStatus(scope.row.status) }}</el-tag>
                                      </template>
                                      <template v-else>
                                          <template v-if="col.type==='datetime'">
                                              <span>{{ datetime.formatDateTime(scope.row[col.key]) }}</span>
                                          </template>
                                          <template v-else>
                                              {{ scope.row[col.key] }}
                                          </template>
                                      </template>
                                    </template>
                                  </template>
                                </template>
                              </template>
                            </template>
                          </template>
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
    import tl from '@/utils/locallize';
    import datetime from '@/utils/datetime';
    

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


    const getUrlImage = (url : any) =>{
        var url_img = `${process.env.VITE_URL}/${url}`
        return url_img
    }
    const getTagTypeStatus = (status: any) => {
      let tagType
      switch (status) {
        case 0: 
          tagType = 'warning'
          break
        case 1: 
          tagType = 'info'
          break
        case 2:
          tagType = 'success'
          break
        case 3:
          tagType = 'danger'
          break
        case 4:
          tagType = 'success'
          break
        default:
          tagType = 'default'
          break
      }
      return tagType
    }

    const getLabelStatus = (status: any) => {
      let label
      switch (status) {
        case 0:
          label = 'Chờ xử lý'
          break
        case 1:
          label = 'Đang xử lý'
          break
        case 2:
          label = 'Đã giao hàng'
          break
        case 3:
          label = 'Đã hủy'
          break
        case 4:
          label = 'Hoàn thành'
          break
        default:
          label = 'Không xác định'
          break
      }
      return label
    }
    
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
