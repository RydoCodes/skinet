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

  incrementItemQuantity(item: IBasketItem): void
  {
    this.basketservice.incrementItemQuantity(item);
  }

  decrementItemQuantity(item: IBasketItem): void{
    this.basketservice.DecrementItemQuantity(item);
  }

  removeBasketItem(item: IBasketItem): void{
    this.basketservice.removeitemfrombasket(item);
  }



}
