import { NgModule } from '@angular/core';
import { ExtraOptions, PreloadAllModules, RouterModule, Routes } from '@angular/router';

import { LotDetailComponent } from './lot-detail.component';
import { LotListComponent } from './lot-list.component';
import { SimulationComponent } from './simulation.component';
import { HomePageComponent } from './home-page.component';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'lot', component: LotDetailComponent },
  { path: 'catalog', component: LotListComponent },
  { path: 'simulation', component: SimulationComponent },
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
