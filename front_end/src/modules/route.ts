import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '@/components/static/DashboardView.vue'
import NotFoundView from '@/components/static/NotFoundView.vue'
import app from '@app/routers/index.app.router.ts'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        // ======================= NOT FOUND =========================
        {
            path: '/404',
            name: 'NotFound',
            component: NotFoundView,
        },

        // ======================= STATIC PAGE =========================
        {
            path: '/',
            name: 'Dashboard',
            component: DashboardView,
            meta: {
                layout: 'Admin',
            },
        },
        ...app,
    ],
})

export default router
