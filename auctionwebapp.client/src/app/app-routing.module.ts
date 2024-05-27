import { NgModule } from '@angular/core';
import { ExtraOptions, PreloadAllModules, RouterModule, Routes } from '@angular/router';

import { LotDetailComponent } from './components/lot-detail/lot-detail.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { SimulationComponent } from './components/simulation/simulation.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginComponent } from './components/login-form/login-form.component';
import { RegistrationComponent } from './components/registration-form/registration-form.component';
import { LotCreateComponent } from './components/lot-create-form/lot-create.component';
/*import { UserProfileComponent } from './components/user-profile/user-profile.compoent';
import { UserLotsComponent } from './components/user-lots/user-lots.component';
import { InfoComponent } from './components/info/info.component';*/

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'lot', component: LotDetailComponent },
  { path: 'catalog', component: LotListComponent },
  { path: 'simulation', component: SimulationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegistrationComponent },
  { path: 'create', component: LotCreateComponent },
  //{ path: 'profile', component: UserProfileComponent },
 // { path: 'lists', component: UserLotsComponent },
  //{ path: 'info', component: InfoComponent },
  { path: '**', redirectTo: '/' }
];

const routerConfig: ExtraOptions = {
  preloadingStrategy: PreloadAllModules,
  scrollPositionRestoration: 'enabled'
};

@NgModule({
  imports: [RouterModule.forRoot(routes, routerConfig)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
