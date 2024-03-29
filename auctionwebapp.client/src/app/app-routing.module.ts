import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LotDetailComponent } from './lot-detail.component';
import { LotListComponent } from './lot-list.component';

const routes: Routes = [
  { path: '', component: LotListComponent },
  { path: 'bid/:id', component: LotDetailComponent },
  { path: '**', redirectTo: '/' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
