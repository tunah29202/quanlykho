import type { App } from 'vue'

import VcMenu from './layout/vc-menu.vue'
import VcRow from './layout/vc-row.vue'
import VcCol from './layout/vc-col.vue'

import VcConfirm from './dialog/vc-confirm.vue'
import VcModal from './dialog/vc-modal.vue'

import VcCard from './card/vc-card.vue'

import VcButton from './form/vc-button.vue'
import VcInput from './form/vc-input.vue'
import VcInputGroup from './form/vc-input-group.vue'
import VcCheckbox from './form/vc-checkbox.vue'
import VcIcon from './form/vc-icon.vue'
import VcTreeview from './form/vc-treeview.vue'
import VcInputIcon from './form/vc-input-icon.vue'

import VcTable from './table/vc-table.vue'
import VcImport from './import/vc-import.vue'

const requireComponent = {
    VcButton,
    VcConfirm,
    VcCheckbox,
    VcInputGroup,
    VcCard,
    VcTable,
    VcModal,
    VcIcon,
    VcRow,
    VcCol,
    VcInput,
    VcInputIcon,
    VcMenu,
    VcTreeview,
    VcImport
}

const register = (app: App<Element>): void => {
    Object.entries(requireComponent).forEach(([name, component]) => {
        app.component(name, component)
    })
}

export default {
    register,
}
