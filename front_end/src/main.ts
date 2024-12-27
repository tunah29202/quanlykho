import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './modules/route'
import { registerLayouts } from './layouts/_register'
import VcRegister from '@/components/commons/vc-register'
import resourceService from '@app/services/core/resource.service'


// ===================== ELEMENT PLUS =========================
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import vi from 'element-plus/dist/locale/vi.mjs'
console.log(import.meta.env.VITE_API_URL);

const app = createApp(App)
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import { json } from 'stream/consumers'
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
VcRegister.register(app)
registerLayouts(app)
const pinia = createPinia()
app.use(pinia)
resourceService.getList({ page: 1, size: 10000, sort: 'screen.asc' })
.then((response: any)=>{
    localStorage.setItem(
        'i18n.ja',
        response.data ? JSON.stringify(response.data) : '[]')    
    app.use(router)
    app.use(ElementPlus, {
        locale: vi,
    })
    app.mount('#app')
})
