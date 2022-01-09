import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { ReactiveFormsModule } from '@angular/forms';
import {BsDropdownModule  } from 'ngx-bootstrap/dropdown';
import { TextInputComponent } from './components/text-input/text-input.component';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent,
    TextInputComponent
  ],
  imports: [
    CommonModule,
   // PaginationModule.forRoot(),
    PaginationModule,
    // so that pagercomponent can make use of it
    ReactiveFormsModule, // used by Login Component currently.
    BsDropdownModule.forRoot()
  ],
  exports: [
    // PaginationModule, // you exported it when you had to use its component straight in shop html and
    // pager component component was not introduced at that time.
    PagingHeaderComponent, // Component used by Shop Component html of Shop Module - Page Name,BreadCrumbs
    PagerComponent, // Component used by Shop Component html of Shop Module - Pagination
    OrderTotalsComponent, // A Component used in Basket component html of Basket Module
    ReactiveFormsModule, // A Module used by Login Component of Account Module gives FormControl, FormGroup, Validators
    BsDropdownModule, // A Module used by Login Component gives Dropdown directive and structural directive
    TextInputComponent
  ]
})
export class SharedModule { }
