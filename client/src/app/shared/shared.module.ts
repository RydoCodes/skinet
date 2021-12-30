import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent
  ],
  imports: [
    CommonModule,
   // PaginationModule.forRoot(),
    PaginationModule,
    // so that pagercomponent can make use of it
  ],
  exports: [
    // PaginationModule, // you exported it when you had to use its component straight in shop html and
    // pager component component was not introduced at that time.
    PagingHeaderComponent,
    PagerComponent, // so that pagination module used in the form of this component can be used in shop component html
    OrderTotalsComponent
  ]
})
export class SharedModule { }
