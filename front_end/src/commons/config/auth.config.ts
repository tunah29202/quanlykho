// ========================== ROUTER =============================

export const ROUTER_AUTH = [
    {
        path: '/login',
        name: 'Login',
        component: () => import('@app/views/auth/LoginView.vue'),
        meta: {
            layout: "Client"
        }
    },
]
// ========================== PATH API =============================

export const API =  {
        SIGN_IN: "auth/login",
        GET_INFO: "auth/info",
        REFRESH_TOKEN: "auth/refresh-token",
        CHANGE_PASSWORD: "auth/change-password",
        SEND_EMAIL: "auth/send-mail",
        CHECK_CODE_PASS: "auth/check-code",
        FORGOT_PASSWORD: "auth/forgot_password",
    }