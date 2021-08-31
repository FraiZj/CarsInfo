import { CarFilterMobileComponent } from './components/car-filter-mobile/car-filter-mobile.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  {
    path: 'mobile',
    component: CarFilterMobileComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarsFilterRoutingModule { }
