import tl from '@/utils/locallize'

export const FUNC_NAME = 'Invoice'

// ========================== ROUTER =============================

export const ROUTER_INVOICE = [
  {
    path: '/invoice',
    name: 'InvoiceList',
    component: () => import('@app/views/invoice/ListView.vue'),
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
    key: "invoice_no",
    title: tl(FUNC_NAME, "invoice_no_text"),
  },
  {
    key: "invoice_date",
    title: tl(FUNC_NAME, "invoice_date_text"),
  },
  {
    key: "shipped_per",
    title: tl(FUNC_NAME, "shipper_text"),
  },
  {
    key: "shipped_date",
    title: tl(FUNC_NAME, "shipped_date_text"),
  },
  {
    key: "consignee",
    title: tl(FUNC_NAME, "consignee_text"),
  },
  {
    key: "status",
    title: tl(FUNC_NAME, "status_text"),
  },
  {
    key: "payment_date",
    title: tl(FUNC_NAME, "payment_date_text"),
  },
]

export default { tableConfig, colConfig }
