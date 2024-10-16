import type { App } from 'vue';
import TheAdminLayout from './TheAdminLayout.vue'
import TheClientLayout from './TheClientLayout.vue'

export function registerLayouts(app: App<Element>) {

    app.component("TheAdminLayout", TheAdminLayout);
    app.component("TheClientLayout", TheClientLayout);

}