import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopRoutingModule } from './shop-routing.module';



@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductDetailsComponent
  ],
  imports: [
    CommonModule, // by default
    SharedModule, // for pagination as of now
    //RouterModule // to add router-link attribute instead of href
    ShopRoutingModule // to add router-link attribute instead of href
  ],
  //exports: [ShopComponent] // we do not need to export this as we will not be importing this module in app module
                          // as app module is no longer responsible for loading the shop component.
})
export class ShopModule { }
