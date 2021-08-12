import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CarsListComponent } from "./components/cars-list/cars-list.component";

const routes: Routes = [
  {
    path: '',
    component: CarsListComponent
  },
  {
    path: ':id',
    loadChildren: () => import('./../car-details/car-details.module').then(m => m.CarDetailsModule)
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarsListRoutingModule { }
