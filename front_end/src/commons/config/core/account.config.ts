// ========================== ROUTER =============================

export const ROUTER_ACCOUNT = [
    {
        path: '/change-password',
        name: 'ChangePassWord',
        component: () => import('@app/views/core/account/ChangePassword.vue'),
    },
]