import tl from '@/utils/locallize'

export const FUNC_NAME = 'Category'

// ========================== ROUTER =============================

export const ROUTER_CATEGORY = [
  {
    path: '/category',
    name: 'CategoryList',
    component: () => import('@app/views/category/ListView.vue'),
  },
]

// ========================== PATH API =============================

export const API = {
  LIST: `/${FUNC_NAME}`,
  CREATE: `/${FUNC_NAME}/create`,
  DETAIL: (id: string) => `${FUNC_NAME}/${id}`,
  UPDATE: (id: string) => `${FUNC_NAME}/update/${id}`,
  DELETE: (id: string | string[]) => `${FUNC_NAME}/delete/${id}`,
  DELETE_MULTI: `${FUNC_NAME}/delete-multi`,
  EXPORT: `${FUNC_NAME}/export-excel`,
}

// ========================== CONFIG TABLE ==========================

export const tableConfig = {
  checkbox: false,
  action: true,
  showPaging: true,
  dbClick: false,
  index: true,
}

export const colConfig = [
  {
    key: "name",
    title: tl(FUNC_NAME, "category_name_text"),
  },
  {
    key: "ingredients",
    title: tl(FUNC_NAME, "ingredient_id_text"),
  },
]

export default { tableConfig, colConfig }
