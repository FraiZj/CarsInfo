import { CarsMainListComponent } from './components/cars-main-list/cars-main-list.component';
import { FavoriteCarsListComponent } from './components/favorite-cars-list/favorite-cars-list.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from '@core/auth/auth-guard';
import { Roles } from '@auth/enums/roles';

const routes: Routes = [
  {
    path: '',
    component: CarsMainListComponent
  },
  {
    path: 'favorite',
    component: FavoriteCarsListComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [Roles.User, Roles.Admin]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarsListRoutingModule { }
