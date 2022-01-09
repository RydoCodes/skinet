import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerformgroup: FormGroup;
  errors: string[];

  constructor(private formbuilder: FormBuilder, private accountservice: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm(): void{
    this.registerformgroup = this.formbuilder.group({
      displayName: [null, [Validators.required]],
      email: ['',
      [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\.)+[\\w -]{2,4}$')],
      [this.validateEmailNotTaken()]
    ],
      password: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    // console.log(this.registerformgroup.value);
    this.accountservice.register(this.registerformgroup.value).subscribe((response) => {
      this.router.navigateByUrl('/shop');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

  // AsyncValidatorFn - A function that receives a control and returns a Promise or observable that emits validation errors
  //                     if present, otherwise null.
  validateEmailNotTaken(): AsyncValidatorFn{
    return control => {
      return timer(500).pipe( // So Its like after half a second do whateevr is happeing under pipe.
        switchMap(() => {
          if (!control.value)
          {
            return of(null); // returning Observable<null>
          }
          return this.accountservice.checkEmailExists(control.value).pipe(
            map((res) => {
              return res ? {emailExists: true} : null;
            })
          );
        })
      );
    };
  }

}
