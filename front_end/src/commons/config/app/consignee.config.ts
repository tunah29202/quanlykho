import tl from '@/utils/locallize'

export const FUNC_NAME = 'Consignee'

// ========================== ROUTER =============================

export const ROUTER_CONSIGNEE = [
  {
    path: '/consignee',
    name: 'ConsigneeList',
    component: () => import('@app/views/consignee/ListView.vue'),
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
    title: tl(FUNC_NAME, "consignee_name_text"),
  },
  {
    key: "address",
    title: tl(FUNC_NAME, "address_text"),
  },
  {
    key: "tax",
    title: tl(FUNC_NAME, "tax_text"),
  },
  {
    key: "email",
    title: tl(FUNC_NAME, "email_text"),
  },
]

export default { tableConfig, colConfig }