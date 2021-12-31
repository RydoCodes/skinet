import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  product: IProduct;
  quantity = 1;
  Exceeded: string = '';

  constructor(private shopservice: ShopService, private activateRoute: ActivatedRoute, private bcservice: BreadcrumbService,
              private basketservice: BasketService) {
    this.bcservice.set('@productDetails', '');
  }

  basketitems$: Observable<IBasket>;

  ngOnInit(): void {
    this.loadProduct();
  }

  addItemToBasket(): void {
    this.basketservice.addItemToBasket(this.product, this.quantity);
  }

  decrementQuantity(): void{
    this.quantity--;
    this.Exceeded = '';
    if (this.quantity === 0)
    {
      this.quantity++;
      this.Exceeded = '';
    }
  }

  incrementQuantity(): void{
    if (this.quantity <= 4)
    {
      this.quantity++;
    }
    if (this.quantity === 5)
    {
      this.Exceeded = 'You cannot select more than 5 products';
    }

  }

  loadProduct(): void{
    this.shopservice.getProduct(+this.activateRoute.snapshot.paramMap.get('id')).subscribe(product => {
      this.product = product;
      this.bcservice.set('@productDetails', product.name);
    }, error => {
      console.log(error);
    });
  }

}
