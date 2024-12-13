import tl from '@/utils/locallize'

export const FUNC_NAME = 'User'

// ========================== ROUTER =============================

export const ROUTER_USER = [
  {
    path: '/user',
    name: 'UserList',
    component: () => import('@app/views/core/user/ListView.vue'),
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
  RESET_PASSWORD: (id: string) => `${FUNC_NAME}/${id}/reset_password`,
  GET_USER_BY_NAME_EMAIL: `${FUNC_NAME}/get-user`,
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
  {
    key: "role_name",
    title: tl(FUNC_NAME, "role_text"),
  },
  
  {
    key: "warehouses",
    title: tl(FUNC_NAME, "warehouse_text"),
  },
]

export default { tableConfig, colConfig }
