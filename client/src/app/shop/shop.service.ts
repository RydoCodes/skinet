import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/producttype';
import {delay, map} from 'rxjs/operators';
import { shopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProducts(shopParams: shopParams) {
    let params = new HttpParams();
    if (shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0){
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if (shopParams.search){
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('PageSize', shopParams.pageSize.toString());


    return this.http.get<IPagination>(this.baseUrl + 'products' , {params})
  }

  getProduct(id: number){
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands(){
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  getTypes(){
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }


}
