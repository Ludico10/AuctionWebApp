import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { JwtModule } from "@auth0/angular-jwt";
import { MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';

import { AppRoutingModule } from './app-routing.module';
import { LotDetailComponent } from './components/lot-detail/lot-detail.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { AppComponent } from './app.component';
import { SimulationComponent } from './components/simulation/simulation.component';
import { SimulationUserFormComponent } from './components/simulation-user-form/simulation-user-form.component';
import { GraficoBarrasComponent } from './components/grafico-barras/grafico-barras.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LotShortComponent } from './components/lot-short/lot-short.component';
import { HeaderComponent } from './components/header/header.component';
import { ModalComponent } from './components/modal/modal.component';
import { CheckAllComponent } from './components/check-all/check-all.component';
import { LoginComponent } from './components/login-form/login-form.component';
import { AuthGuard } from './guards/auth.guard';
import { LotCreateComponent } from './components/lot-create-form/lot-create.component';
import { RegistrationComponent } from './components/registration-form/registration-form.component';
import { ImageUploadComponent } from './components/image-upload/image-upload-component';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
//import { InfoComponent } from './components/info/info.component';
//import { UserLotsComponent } from './components/user-lots/user-lots.component';
//import { UserProfileComponent } from './components/user-profile/user-profile.compoent';

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
    RegistrationComponent,
    LotCreateComponent,
    ImageUploadComponent,
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    MatRadioModule, CommonModule,
    MatCheckboxModule, MatPaginatorModule,
    MatFormFieldModule, MatSelectModule,
    MatDatepickerModule, ReactiveFormsModule,
    NgxMaterialTimepickerModule, MatInputModule, 
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7183"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [AuthGuard, provideAnimationsAsync(), provideNativeDateAdapter()],
  bootstrap: [AppComponent]
})
export class AppModule { }
