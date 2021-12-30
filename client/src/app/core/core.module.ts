import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { NgxSpinnerModule } from 'ngx-spinner';



@NgModule({
  declarations: [NavBarComponent, TestErrorComponent, NotFoundComponent, ServerErrorComponent, SectionHeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    // toastr service : Used in Error Interceptor and not toaster module anywhere.
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-left',
      preventDuplicates: true
    }),
    // BreadCrumb Service : Used to change breadcrumb name in Product detail component.
    BreadcrumbModule // This module gives <xng-breadcrumb> component to use inside SectionHeaderComponent
  ],
  exports: [NavBarComponent, SectionHeaderComponent]
})
export class CoreModule { }

// test-error  component  : To show 400 Validation Error thrown back to this component by Error Interceptor
// server-error component : To show 500 Internal Server Error Information redirected by Error Interceptor passing
//                          a navigation extra containing error object.

