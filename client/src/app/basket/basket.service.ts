import { HttpClient } from '@angular/common/http';
import { isNgTemplate, ThrowStmt } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root' // It is injected to our app module at startup
})
export class BasketService {

  baseUrl = environment.apiURL;

  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$: Observable<IBasket> = this.basketSource.asObservable();

  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$: Observable<IBasketTotals> = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) {
  }

  // Currently getBasket is being Subscribed only from app.component.ts
  getBasket(id: string): Observable<void> {
    return this.http.get<IBasket>(this.baseUrl + 'basket?Id=' + id)
    .pipe(
      map((response: IBasket) => {
        this.basketSource.next(response); // I think Next sets up the value of basketsource which is a readonly property.
        this.calculateTotals();
        // console.log(this.getCurrentBasketValue());
      })
    );
  }

  // Currently getBasket is being called only via addItemToBasket at the end.
  // setBasket is Subscribed in the service only and is not called from any other ts file.
  setBasket(basket: IBasket): Subscription {
    return this.http.post<IBasket>(this.baseUrl + 'basket', basket)
    .subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateTotals();
      // console.log(response);
    }, error => {
      console.log(error);
    });
  }

  getCurrentBasketValue(): IBasket | null {
    return this.basketSource.value;
  }

  // called when you click on add to card button
  addItemToBasket(item: IProduct, quantity= 1): void {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    // basket.items.push(itemToAdd);
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
}
  // This returns updated list of IBasketItem
  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    console.log(items);
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) // meaning there is no item with this id and now we push it
    {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    else{ // meaning there is an item with this id and now we do not push it. we increase the quantity.
      items[index].quantity += quantity;
      // At this point, quantity property of existing item was updated to ++ and then pushed back to items array of basket.
    }
    return items;

  }

createBasket(): IBasket {
  const basket = new Basket();
  localStorage.setItem('basket_id', basket.id);
  return basket;
}


private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
  return {
    id: item.id,
    productName: item.name,
    price: item.price,
    pictureUrl: item.pictureUrl,
    brand: item.productBrand,
    quantity,
    type: item.productType
  };
}


  // called by app.module.ts -> getBasket() ->
  private calculateTotals(): void{
    const basket = this.getCurrentBasketValue();
    const shipping = 10;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    // reduce: Calls the specified callback function for all the elements in an array.
    // The return value of the callback function is the accumulated result,
    // and is provided as an argument in the next call to the callback function.
    const total = shipping + subtotal;
    this.basketTotalSource.next({shipping, subtotal, total});
  }

  incrementItemQuantity(item: IBasketItem): void{
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  DecrementItemQuantity(item: IBasketItem): void{
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1)
    {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }
    else{
      this.removeitemfrombasket(item);
    }
  }
  // At this point, we check the lenght of baskitems after removing the basket item to remove.
  // If the length is =0 then it means that the basketitem array is empty and we should delete the basket.
  // If the length = 1 then it means we just have to update the basket removing that item.
  removeitemfrombasket(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    // some : Determines whether the specified callback function returns true for any element of an array.
    if (basket.items.find(i => i.id === item.id)){
        basket.items = basket.items.filter(x => x.id !== item.id);
        if (basket.items.length > 0)
        {
          // after removing the current basketitem, we do have other basketitems
          this.setBasket(basket);
        }
        else
        {
          // after removing the current basketitem , we do not have any basket item and that means that the basket is empty.
          this.deleteBasket(basket);
        }
    }
  }
  deleteBasket(basket: IBasket): void {
    this.http.delete<boolean>(this.baseUrl + 'basket?id=' + basket.id).subscribe(
      () => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem('basket_id');
      }, error => {
        console.log(error);
      }
    );
  }


}

