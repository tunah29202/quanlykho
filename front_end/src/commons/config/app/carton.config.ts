import tl from '@/utils/locallize'

export const FUNC_NAME = 'Carton'
export const PERMISION_NAME = 'CARTON'

// ========================== ROUTER =============================

export const ROUTER_CARTON = [
  {
    path: '/carton',
    name: 'CartonList',
    component: () => import('@app/views/app/carton/ListView.vue'),
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
}

export const PERMISSION = {
  LIST: `${PERMISION_NAME}_LIST`,
  CREATE: `${PERMISION_NAME}_CREATE`,
  UPDATE: `${PERMISION_NAME}_UPDATE`,
  DETAIL: `${PERMISION_NAME}_DETAIL`,
  DELETE: `${PERMISION_NAME}_DELETE`,
}

// ========================== CONFIG TABLE ==========================

export const tableConfig = {
  checkbox: false,
  action: true,
  showPaging: true,
  dbClick: false,
  index: true,
  expand: true
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
    key: "carton_no",
    title: tl(FUNC_NAME, "carton_no_text"),
  },
  {
    key: "width",
    title: tl(FUNC_NAME, "width_text"),
  },
  {
    key: "height",
    title: tl(FUNC_NAME, "height_text"),
  },
  {
    key: "length",
    title: tl(FUNC_NAME, "length_text"),
  },
  {
    key: "gross_weight",
    title: tl(FUNC_NAME, "gross_weight_text"),
  },
  {
    key: "net_weight",
    title: tl(FUNC_NAME, "net_weight_text"),
  },
  {
    key: "volume",
    title: tl(FUNC_NAME, "volume_text"),
  },
  {
    key: "total_amount",
    title: tl(FUNC_NAME, "total_amount_text"),
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
