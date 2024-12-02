import tl from '@/utils/locallize';
export default[
    { routerName: 'DashBoard', path: '/', text: tl('SideBar', 'DashBoard'), icon: 'House'},
    { routerName: 'userManageMent', path: '/user', text: tl('SideBar', 'user_management'), icon: 'User'},
    { routerName: 'roleManageMent', path: '/role', text: tl('SideBar', 'role_management'), icon: 'House'},
    { routerName: 'functionManageMent', path: '/function', text: tl('SideBar', 'function_management'), icon: 'House'},
    { routerName: 'customerManageMent', path: '/customer', text: tl('SideBar', 'customer_management'), icon: 'Place'},
    { routerName: 'shipperManageMent', path: '/shipper', text: tl('SideBar', 'shipper_management'), icon: 'Van'},
    { routerName: 'warehouseManageMent', path: '/warehouse', text: tl('SideBar', 'warehouse_management'), icon: 'MessageBox'},
    { routerName: 'productManageMent', path: '/product', text: tl('SideBar', 'product_management'), icon: 'Files'},
    { routerName: 'categoryManageMent', path: '/category', text: tl('SideBar', 'category_management'), icon: 'TakeawayBox'},
    { routerName: 'cartonManageMent', path: '/carton', text: tl('SideBar', 'carton_management'), icon: 'Box'},
    { routerName: 'invoiceManageMent', path: '/invoice', text: tl('SideBar', 'invoice_management'), icon: 'Tickets'},
    { routerName: 'orderManageMent', path: '/order', text: tl('SideBar', 'order_management'), icon: 'Tickets'},
]