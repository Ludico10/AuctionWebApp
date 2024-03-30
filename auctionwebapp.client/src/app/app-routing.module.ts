import { NgModule } from '@angular/core';
import { ExtraOptions, PreloadAllModules, RouterModule, Routes } from '@angular/router';

import { LotDetailComponent } from './lot-detail.component';
import { LotListComponent } from './lot-list.component';

const routes: Routes = [
  { path: '', component: LotListComponent },
  { path: 'lots/:id', component: LotDetailComponent },
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
