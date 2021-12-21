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
    // you are actually using toastr service and not toaster module anywhere.
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-left',
      preventDuplicates: true
    }),
    BreadcrumbModule
  ],
  exports: [NavBarComponent, SectionHeaderComponent]
})
export class CoreModule { }

// test-error  component  : making Bad API Calls AND to show 400 Validation Error thrown back to this component from Error Interceptor
// server-error component : to show 500 Internal Server Error Information redirected by Error Interceptor

