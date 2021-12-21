import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent
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
    PagerComponent
  ]
})
export class SharedModule { }
