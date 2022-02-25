import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/producttype';
import { shopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  @ViewChild('search', {static: false}) searchterm: ElementRef;

  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  totalCount: number; // not a part of shop params.

  shopParams = new shopParams();

  sortOptions: any[] = [
    {name: 'Alphabetical', value: 'defaultisbyname'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ];


  constructor(private shopservice: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts(): void{
    this.shopservice.getProducts(this.shopParams).subscribe((response: IPagination) => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;

    }, error => {
      console.log(error);
    }, );
  }

  getBrands(): void{
    this.shopservice.getBrands().subscribe((response: IBrand[]) => {
        this.brands = [{id: 0, name: 'All'}, ...response];
    }, error => {
        console.log(error);
    });
  }

  getTypes(): void{
    this.shopservice.getTypes().subscribe((response: IType[]) => {
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  onBrandSelected(brandId: number): void{
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
     // to avoid error- NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked.
     // Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onTypeSelected(typeId: number): void{
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    // to avoid error- NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked.
    // Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onSortSelected(sort: string): void{
    this.shopParams.sort = sort;
    this.shopParams.pageNumber = 1; // to avoid Error - NG0100: ExpressionChangedAfterItHasBeenCheckedError:
    // Expression has changed after it was checked. Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onSearch(): void{
    this.shopParams.search = this.searchterm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    // to avoid error - NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked.
    // Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onPageChanged(event: any): void{
    if (this.shopParams.pageNumber !== event){ // meaing we changed the page using the pagination buttton
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onReset(): void{
    this.searchterm.nativeElement.value = ' '; // resetting the search input
    this.shopParams = new shopParams(); // reseting all shopparams class properties to their default values.
    this.getProducts(); // Gets list of unfiltered products
  }

}
