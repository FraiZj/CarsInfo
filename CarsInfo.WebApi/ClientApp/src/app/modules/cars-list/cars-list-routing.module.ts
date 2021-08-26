import { FavoriteCarsListComponent } from './components/favorite-cars-list/favorite-cars-list.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CarsListComponent } from "./components/cars-list/cars-list.component";
import { AuthGuard } from '@core/auth/auth-guard';

const routes: Routes = [
  {
    path: '',
    component: CarsListComponent
  },
  {
    path: 'favorite',
    component: FavoriteCarsListComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarsListRoutingModule { }
