import tl from '@/utils/locallize'

export const FUNC_NAME = 'Product'

// ========================== ROUTER =============================

export const ROUTER_PRODUCT = [
  {
    path: '/product',
    name: 'ProductList',
    component: () => import('@app/views/app/product/ListView.vue'),
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
    key: "code",
    title: tl(FUNC_NAME, "code_text"),
  },
  {
    key: "name",
    title: tl(FUNC_NAME, "name_text"),
  },
  {
    key: "origin",
    title: tl(FUNC_NAME, "origin_text"),
  },
  {
    key: "price_unit",
    title: tl(FUNC_NAME, "price_unit_text"),
  },
  {
    key: "quantity",
    title: tl(FUNC_NAME, "quantity_text"),
  },
  {
    key: "ingredient",
    title: tl(FUNC_NAME, "ingredient_text"),
  },{
    key: "status",
    title: tl(FUNC_NAME, "status_text"),
  }
]

export default { tableConfig, colConfig }
