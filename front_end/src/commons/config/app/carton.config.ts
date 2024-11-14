import tl from '@/utils/locallize'

export const FUNC_NAME = 'Carton'

// ========================== ROUTER =============================

export const ROUTER_CARTON = [
  {
    path: '/carton',
    name: 'CartonList',
    component: () => import('@app/views/carton/ListView.vue'),
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
    key: "carton_no",
    title: tl(FUNC_NAME, "carton_no_text"),
  },
  {
    key: "net_weight",
    title: tl(FUNC_NAME, "net_weight_text"),
  },
  {
    key: "gross_weight",
    title: tl(FUNC_NAME, "gross_weight_text"),
  },
  {
    key: "length",
    title: tl(FUNC_NAME, "length_text"),
  },
  {
    key: "total_amount",
    title: tl(FUNC_NAME, "total_amount_text"),
  },
  {
    key: "customer",
    title: tl(FUNC_NAME, "phone_text"),
  },
]

export default { tableConfig, colConfig }
