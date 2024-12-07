import tl from '@/utils/locallize'

export const FUNC_NAME = 'Shipper'

// ========================== ROUTER =============================

export const ROUTER_SHIPPER = [
  {
    path: '/shipper',
    name: 'ShipperList',
    component: () => import('@app/views/app/shipper/ListView.vue'),
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
    title: tl(FUNC_NAME, "shipper_name_text"),
  },
  {
    key: "address",
    title: tl(FUNC_NAME, "address_text"),
  },
  {
    key: "tel",
    title: tl(FUNC_NAME, "tel_text"),
  },
  {
    key: "fax",
    title: tl(FUNC_NAME, "fax_text"),
  },
  {
    key: "email",
    title: tl(FUNC_NAME, "email_text"),
  },
]

export default { tableConfig, colConfig }
