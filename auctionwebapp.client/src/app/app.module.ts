import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { JwtModule } from "@auth0/angular-jwt";

import { AppRoutingModule } from './app-routing.module';
import { LotDetailComponent } from './components/lot-detail/lot-detail.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { AppComponent } from './app.component';
import { SimulationComponent } from './components/simulation/simulation.component';
import { SimulationUserFormComponent } from './components/simulation-user-form/simulation-user-form.component';
import { GraficoBarrasComponent } from './components/grafico-barras/grafico-barras.component';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LotShortComponent } from './components/lot-short/lot-short.component';
import { HeaderComponent } from './components/header/header.component';
import { ModalComponent } from './components/modal/modal.component';
import { CheckAllComponent } from './components/check-all/check-all.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

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
    LoginComponent,
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    MatRadioModule, CommonModule,
    MatCheckboxModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7183"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
