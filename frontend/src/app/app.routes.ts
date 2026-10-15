import { Routes } from '@angular/router';
import { OrderListComponent } from './components/order-list/order-list';
import { OrderDetailComponent } from './components/order-detail/order-detail';
import { CreateOrderComponent } from './components/create-order/create-order';
import { ProductCatalogComponent } from './components/product-catalog/product-catalog';
import { CreateCustomerComponent } from './components/create-customer/create-customer';
import { CreateProductComponent } from './components/create-product/create-product';
import { LoginComponent } from './components/login/login';
import { AdminUsersComponent } from './components/admin-users/admin-users';
import { authGuard, adminGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'admin/users', component: AdminUsersComponent, canActivate: [authGuard, adminGuard] },
  { path: '', component: OrderListComponent, canActivate: [authGuard] },
  { path: 'create', component: CreateOrderComponent, canActivate: [authGuard] },
  { path: 'orders/:id', component: OrderDetailComponent, canActivate: [authGuard] },
  { path: 'products', component: ProductCatalogComponent, canActivate: [authGuard] },
  { path: 'products/create', component: CreateProductComponent, canActivate: [authGuard] },
  { path: 'customers/create', component: CreateCustomerComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
