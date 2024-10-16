export enum ResponseCode {
    Success = 0,
    SystemError = 1,
    NotFound = 2,
    Invalid = 3,
    UnAuthorized = 4,
}

export enum COOKIE_KEY {
    ACCESS_TOKEN = 'auth.access_token',
    REFRESH_TOKEN = 'auth.refresh_token',
}

export enum POPUP_TYPE {
    VIEW = 'VIEW',
    CREATE = 'CREATE',
    EDIT = 'EDIT',
}

export const ERROR_TEXT = 'error'

