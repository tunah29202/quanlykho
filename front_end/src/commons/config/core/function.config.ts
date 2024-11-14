import tl from '@/utils/locallize'

export const FUNC_NAME = 'Function'

// ========================== ROUTER =============================

export const ROUTER_FUNCTION = [
  {
    path: '/function',
    name: 'FunctionList',
    component: () => import('@app/views/function/ListView.vue'),
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
    title: tl(FUNC_NAME, "user_code_text"),
  },
  {
    key: "full_name",
    title: tl(FUNC_NAME, "full_name_text"),
  },
  {
    key: "gender",
    title: tl(FUNC_NAME, "gender_text"),
  },
  {
    key: "phone",
    title: tl(FUNC_NAME, "phone_text"),
  },
]

export default { tableConfig, colConfig }
