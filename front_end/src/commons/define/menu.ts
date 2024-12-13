import tl from '@/utils/locallize';

export default [
    { 
        routerName: 'DashBoard', 
        path: '/', 
        text: tl('SideBar', 'dashboard'), 
        icon: 'House', 
    },
    { 
        routerName: 'productManageMent', 
        path: '/product', 
        text: tl('SideBar', 'product_management'), 
        icon: 'Files', 
        permission: 'PRODUCT_LIST' 
    },
    { 
        routerName: 'categoryManageMent', 
        path: '/category', 
        text: tl('SideBar', 'category_management'), 
        icon: 'TakeawayBox', 
        permission: 'CATEGORY_LIST' 
    },
    { 
        routerName: 'cartonManageMent', 
        path: '/carton', 
        text: tl('SideBar', 'carton_management'), 
        icon: 'Box', 
        permission: 'CARTON_LIST' 
    },
    { 
        routerName: 'invoiceManageMent', 
        path: '/invoice', 
        text: tl('SideBar', 'invoice_management'), 
        icon: 'Tickets', 
        permission: 'INVOICE_LIST' 
    },
    { 
        routerName: 'orderManageMent', 
        path: '/order', 
        text: tl('SideBar', 'order_management'), 
        icon: 'Memo', 
        permission: 'ORDER_LIST' 
    },
    { 
        routerName: 'customerManageMent', 
        path: '/customer', 
        text: tl('SideBar', 'customer_management'), 
        icon: 'Avatar', 
        permission: 'CUSTOMER_LIST' 
    },
    { 
        routerName: 'shipperManageMent', 
        path: '/shipper', 
        text: tl('SideBar', 'shipper_management'), 
        icon: 'Van', 
        permission: 'SHIPPER_LIST' 
    },
    { 
        routerName: 'warehouseManageMent', 
        path: '/warehouse', 
        text: tl('SideBar', 'warehouse_management'), 
        icon: 'MessageBox', 
        permission: 'WAREHOUSE_UPDATE' 
    },
    { 
        routerName: 'userManageMent', 
        path: '/user', 
        text: tl('SideBar', 'user_management'), 
        icon: 'User', 
        permission: 'USER_LIST' 
    },
    { 
        routerName: 'roleManageMent', 
        path: '/role', 
        text: tl('SideBar', 'role_management'), 
        icon: 'Edit', 
        permission: 'ROLE_LIST' 
    },
    { 
        routerName: 'functionManageMent', 
        path: '/function', 
        text: tl('SideBar', 'function_management'), 
        icon: 'Setting', 
        permission: 'FUNCTION_LIST' 
    },
];
