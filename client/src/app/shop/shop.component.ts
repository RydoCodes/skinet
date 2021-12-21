import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IBrand } from '../shared/models/brands';
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

  sortOptions = [
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

  getProducts(){
    this.shopservice.getProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;

    }, error => {
      console.log(error);
    },);
  }

  getBrands(){
    this.shopservice.getBrands().subscribe(response => {
        this.brands = [{id: 0, name: 'All'}, ...response];
    }, error => {
        console.log(error);
    });
  }

  getTypes(){
    this.shopservice.getTypes().subscribe(response => {
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  onBrandSelected(brandId: number){
    this.shopParams.brandId = brandId;
     this.shopParams.pageNumber=1; // to avoid error- NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onTypeSelected(typeId: number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber=1; // to avoid error- NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onSortSelected(sort: string){
    this.shopParams.sort = sort;
    this.shopParams.pageNumber=1; // to avoid Error - NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onSearch(){
    this.shopParams.search = this.searchterm.nativeElement.value;
    this.shopParams.pageNumber=1; // to avoid error - NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: '3'. Current value: '1'..
    this.getProducts();
  }

  onPageChanged(event: any){
    if(this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onReset(){
    this.searchterm.nativeElement.value = ""; // resetting the search input
    this.shopParams = new shopParams(); // reseting all shopparams class properties to their default values.
    this.getProducts(); // Gets list of unfiltered products
  }

}
