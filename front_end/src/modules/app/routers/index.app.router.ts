import { ROUTER_AUTH } from '@/commons/config/auth.config'
import { ROUTER_ACCOUNT } from '@/commons/config/account.config'
import { ROUTER_USER } from '@/commons/config/user.config'

let routers = [
    ...ROUTER_AUTH,
    ...ROUTER_ACCOUNT,
    ...ROUTER_USER,

]

export default routers;