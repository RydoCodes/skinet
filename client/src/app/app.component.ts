import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit   {

  title = 'Skinet';

  constructor(private basketservice: BasketService, private accountservice: AccountService) {

  }

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const token = localStorage.getItem('token');
    this.accountservice.loadCurrentUser(token).subscribe(() => {
      console.log('user loaded');
    }, (error) => {
      console.log(error);
    });
  }

  loadBasket(): void{
    const basketId = localStorage.getItem('basket_id');
    // Returns the current value associated with the given key, or null if the given key does not exist.

    if (basketId){
      // getBasket does not return any observable
      this.basketservice.getBasket(basketId).subscribe(() => {
        console.log('basket initialised');
      }, error => {
          console.log(error);
      });
    }
  }
}
