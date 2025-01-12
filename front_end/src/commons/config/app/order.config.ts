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
  GET_NOT_IN_INVOICE: `${FUNC_NAME}/get-not-in-invoice`,
  CREATE: `/${FUNC_NAME}/create`,
  DETAIL: (id: string) => `${FUNC_NAME}/${id}`,
  UPDATE: (id: string) => `${FUNC_NAME}/update/${id}`,
  DELETE: (id: string | string[]) => `${FUNC_NAME}/delete/${id}`,
  DELETE_MULTI: `${FUNC_NAME}/delete-multi`,
  EXPORT: `${FUNC_NAME}/export-excel`,
  GET_ORDER_NO: `/${FUNC_NAME}/get-order-no`,
  STATISTICAL: `/${FUNC_NAME}/statistical`
}

// ========================== CONFIG TABLE ==========================

export const tableConfig = {
  checkbox: false,
  action: true,
  showPaging: true,
  dbClick: false,
  index: true,
  expand: true,
  order:true,
}

export const tableModalConfig = {
  checkbox: false,
  action: true,
  showPaging: true,
  dbClick: false,
  index: false,
  expand: false
}

export const colConfig = [
  {
    key: "order_no",
    title: tl(FUNC_NAME, "order_no_text"),
  },
  {
    key: "customer",
    title: tl(FUNC_NAME, "customer_company_text"),
  },
  {
    key: "total_amount",
    title: tl(FUNC_NAME, "total_amount_text"),
  },
  {
    key: "order_date",
    type:'datetime',
    title: tl(FUNC_NAME, "order_date_text"),
  },
  {
    key: "status",
    title: tl(FUNC_NAME, "status_text"),
  },
]

export const colConfigLeft = [
  {
    key: "code",
    title: tl("Product", "code_text"),
  },
  {
    key: "name",
    title: tl("Product", "name_text"),
  },
  {
    key: "quantity",
    title: tl("Product", "quantity_text"),
  },
  {
    key: "price_unit",
    title: tl("Product", "price_unit_text"),
  },
]


export default { tableConfig, colConfig, colConfigLeft, tableModalConfig}
