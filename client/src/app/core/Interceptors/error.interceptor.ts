import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, delay } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { BusyService } from '../services/busy.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService, private busyservice: BusyService) {}

  // next of type HttpHandler is http response coming back.
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      // delay(1000),
       catchError( error => {
          if (error){
            // validation error
            if (error.status === 400 ){
              if (error.error.errors){ // meaning this is 400 validation error
                  throw error.error; // throw our own error object back to the Error Component
                }
                else // meaning this 400 Not Found Request
                {
                  this.toastr.error(error.error.message, error.error.statusCode);
                }
              }
            // UnAuthorissed
            if (error.status === 401){
              this.toastr.error(error.error.message, error.error.statusCode);
            }
            // not found error
            if (error.status === 404){
              this.router.navigateByUrl('/not-found');
            }
            // internal server error
            if (error.status === 500){
              const navigationExtras: NavigationExtras = {state: {rydoerror: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
            }
          }
          return throwError(error);
       })
    );
  }
}
