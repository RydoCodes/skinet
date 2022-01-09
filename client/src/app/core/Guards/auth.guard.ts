import { ThrowStmt } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountservice: AccountService, private router: Router) {

  }

  // route : The route that is attempting to be activated.
  // State  : defines the state of current routerstate and we could use this to find out where the user is coming from.
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.accountservice.currentUser$.pipe(
      map(auth => {
        if (auth){
          return true;
        }
        this.router.navigate(['account/login'], {queryParams: {rydoreturnUrl: state.url}});
      })
    );
  }

}
