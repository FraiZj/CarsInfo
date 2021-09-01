import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarCreationComponent } from './components/car-creation/car-creation.component';

const routes: Routes = [
  {
    path: '',
    component: CarCreationComponent,

  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarCreationRoutingModule { }
