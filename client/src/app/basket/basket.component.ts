import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from '../shared/models/basket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {

  basket$: Observable<IBasket>;

  constructor(private basketservice: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketservice.basket$;
  }

  // Not a service call
  incrementItemQuantity(item: IBasketItem): void
  {
    this.basketservice.incrementItemQuantity(item);
  }

  // Not a service call
  decrementItemQuantity(item: IBasketItem): void{
    this.basketservice.DecrementItemQuantity(item);
  }

  // Not a service call
  removeBasketItem(item: IBasketItem): void{
    this.basketservice.removeitemfrombasket(item);
  }



}
