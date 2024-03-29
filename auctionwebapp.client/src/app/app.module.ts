import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { LotDetailComponent } from './lot-detail.component';
import { LotListComponent } from './lot-list.component';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    LotDetailComponent,
    LotListComponent,
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
