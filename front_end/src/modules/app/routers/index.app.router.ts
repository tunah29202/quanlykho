import { ROUTER_AUTH } from '@/commons/config/core/auth.config'
import { ROUTER_ACCOUNT } from '@/commons/config/core/account.config'
import { ROUTER_USER } from '@/commons/config/core/user.config'
import { ROUTER_ROLE } from '@/commons/config/core/role.config'
import { ROUTER_FUNCTION } from '@/commons/config/core/function.config'
import { ROUTER_CARTON } from '@/commons/config/app/carton.config'
import { ROUTER_ORDER } from '@/commons/config/app/order.config'
import { ROUTER_CATEGORY } from '@/commons/config/app/category.config'
import { ROUTER_CUSTOMER } from '@/commons/config/app/customer.config'
import { ROUTER_INVOICE } from '@/commons/config/app/invoice.config'
import { ROUTER_PRODUCT } from '@/commons/config/app/product.config'
import { ROUTER_SHIPPER } from '@/commons/config/app/shipper.config'
import { ROUTER_WAREHOUSE } from '@/commons/config/app/warehouse.config'

let routers = [
    ...ROUTER_ROLE,
    ...ROUTER_FUNCTION,
    ...ROUTER_AUTH,
    ...ROUTER_ACCOUNT,
    ...ROUTER_CARTON,
    ...ROUTER_ORDER,
    ...ROUTER_CATEGORY,
    ...ROUTER_USER,
    ...ROUTER_CUSTOMER,
    ...ROUTER_INVOICE,
    ...ROUTER_PRODUCT,
    ...ROUTER_SHIPPER,
    ...ROUTER_WAREHOUSE,

]

export default routers;