import tl from '@/utils/locallize'

export const FUNC_NAME = 'Invoice'

// ========================== ROUTER =============================

export const ROUTER_INVOICE = [
  {
    path: '/invoice',
    name: 'InvoiceList',
    component: () => import('@app/views/app/invoice/ListView.vue'),
  },

  {
    path: '/invoice/:id/view',
    name: 'ViewInvoice',
    component: () => import('@app/views/app/invoice/DetailView.vue'),
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
  EXPORT: (id: string) => `${FUNC_NAME}/export-invoice/${id}`,
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
    key: "order_no",
    title: tl(FUNC_NAME, "order_no_text"),
  },
  {
    key: "customer_company",
    title: tl(FUNC_NAME, "customer_company_text"),
  },
  {
    key: "invoice_date",
    title: tl(FUNC_NAME, "invoice_date_text"),
  },
  {
    key: "shipped_date",
    title: tl(FUNC_NAME, "shipped_date_text"),
  },
  {
    key: "status",
    title: tl(FUNC_NAME, "status_text"),
  },
  {
    key: "payment_method",
    title: tl(FUNC_NAME, "payment_method_text"),
  },
]

export const colTabInvoiceConfig = [
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
    key: "unit",
    title: tl(FUNC_NAME, "unit_text"),
  },
  {
    key: "quantity",
    title: tl(FUNC_NAME, "quantity_text"),
  },
  {
    key: "total_amount",
    title: tl(FUNC_NAME, "total_amount_text"),
  },
]

export const colTabCartonConfig = [
  {
    key: "image",
    title: tl(FUNC_NAME, "image_text"),
  },
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
    key: "unit",
    title: tl(FUNC_NAME, "unit_text"),
  },
  {
    key: "quantity",
    title: tl(FUNC_NAME, "quantity_text"),
  },
  {
    key: "total_amount",
    title: tl(FUNC_NAME, "total_amount_text"),
  },
]

export default { tableConfig, colConfig, colTabInvoiceConfig, colTabCartonConfig }
