import tl from '@/utils/locallize'

export const FUNC_NAME = 'Order'

// ========================== ROUTER =============================

export const ROUTER_ORDER = [
  {
    path: '/order',
    name: 'OrderList',
    component: () => import('@app/views/app/order/ListView.vue'),
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
    key: "order_no",
    title: tl(FUNC_NAME, "order_no_text"),
  },
  {
    key: "customer_company",
    title: tl(FUNC_NAME, "customer_company_text"),
  },
  {
    key: "product_name",
    title: tl(FUNC_NAME, "product_name_text"),
  },
  {
    key: "quantity",
    title: tl(FUNC_NAME, "quantity_text"),
  },
  {
    key: "unit",
    title: tl(FUNC_NAME, "unit_text"),
  },
  {
    key: "total_amount",
    title: tl(FUNC_NAME, "total_amount_text"),
  },
  {
    key: "status",
    title: tl(FUNC_NAME, "status_text"),
  },
]

export default { tableConfig, colConfig }
