import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {HttpClientModule} from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { ShopModule } from './shop/shop.module';
import { HomeModule } from './home/home.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, // This contains the routermodule which contains the parent routes aray
    BrowserAnimationsModule,
    HttpClientModule,
    CoreModule, // This contains the Navigation Bar Component.
    //ShopModule // we do not need to load shopmodule as we are only loading this when we go to route /shop
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
