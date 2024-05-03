import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';

import { AppRoutingModule } from './app-routing.module';
import { LotDetailComponent } from './lot-detail.component';
import { LotListComponent } from './lot-list.component';
import { AppComponent } from './app.component';
import { SimulationComponent } from './simulation.component';
import { SimulationUserFormComponent } from './simulation-user-form.component';
import { GraficoBarrasComponent } from './grafico-barras.component';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HomePageComponent } from './home-page.component';
import { LotShortComponent } from './lot-short.component';
import { HeaderComponent } from './header.component';
import { ModalComponent } from './modal.component';
import { CheckAllComponent } from './check-all.component';

@NgModule({
  declarations: [
    LotDetailComponent,
    LotListComponent,
    GraficoBarrasComponent,
    SimulationUserFormComponent,
    SimulationComponent,
    HomePageComponent,
    LotShortComponent,
    HeaderComponent,
    ModalComponent,
    CheckAllComponent,
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    MatRadioModule, CommonModule,
    MatCheckboxModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
