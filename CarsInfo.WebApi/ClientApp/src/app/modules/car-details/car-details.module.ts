import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarDetailsComponent } from './components/car-details/car-details.component';
import { MatCardModule } from '@angular/material/card';
import { CarsDetailsRoutingModule } from './car-details-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    CarDetailsComponent
  ],
  imports: [
    // library modules
    MatCardModule,
    MatButtonModule,
    CommonModule,

    // app modules
    CarsDetailsRoutingModule,
    SharedModule
  ],
  exports: [
    CarsDetailsRoutingModule
  ]
})
export class CarDetailsModule { }
