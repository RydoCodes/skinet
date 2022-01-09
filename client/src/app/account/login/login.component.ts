import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginFormgroup: FormGroup;
  returnUrl: string;


  constructor(private accountService: AccountService, private router: Router, private activatedroute: ActivatedRoute) { }

  // +this.activateRouted.snapshot.paramMap.get('id') : Use param
  ngOnInit(): void {
    this.returnUrl = this.activatedroute.snapshot.queryParams.rydoreturnUrl || '/shop';
    this.createLoginForm();
  }

  createLoginForm(): void {
      this.loginFormgroup = new FormGroup({
        email: new FormControl('', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\.)+[\\w -]{2,4}$')]),
        password: new FormControl('', Validators.required) // first parameter of FormControl is the Initial Value
      });
  }

  onSubmit(): void{
    // console.log(this.loginForm.value);
    this.accountService.login(this.loginFormgroup.value)
    .subscribe(() => {
      console.log('user logged in');
      // this.router.navigateByUrl('/shop');
      this.router.navigateByUrl(this.returnUrl);
    }, error => {
      console.log(error);
    });
  }

}
