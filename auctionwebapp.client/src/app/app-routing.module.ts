import { NgModule } from '@angular/core';
import { ExtraOptions, PreloadAllModules, RouterModule, Routes } from '@angular/router';

import { LotDetailComponent } from './components/lot-detail/lot-detail.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { SimulationComponent } from './components/simulation/simulation.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginComponent } from './components/login/login.component';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'lot', component: LotDetailComponent },
  { path: 'catalog', component: LotListComponent },
  { path: 'simulation', component: SimulationComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: 'catalog' }
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
