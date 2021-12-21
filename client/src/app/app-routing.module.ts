import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { ShopComponent } from './shop/shop.component';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},
  {path: 'test-error', component: TestErrorComponent, data: {breadcumb: 'Test Errors'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Error'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'}}, // in core.module.ts
  {path: 'shop', loadChildren: ()  => import('./shop/shop.module').then(mod => mod.ShopModule), data: {breadcrumb: 'Shop'}},
  {path: '**', redirectTo:  'not-found' , pathMatch: 'full' } // this will redirect to home page if there is any bad URL
];
// you do not need to export serverError and NotFound Components
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
