import tl from '@/utils/locallize'

export const FUNC_NAME = 'Resource'
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
    title: tl(FUNC_NAME, "code_text"),
  },
  {
    key: "name",
    title: tl(FUNC_NAME, "name_text"),
  },
  {
    key: "description",
    title: tl(FUNC_NAME, "description_text"),
  },
]

export default { tableConfig, colConfig }
