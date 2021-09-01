import { SharedModule } from './../shared/shared.module';
import { CarsModule } from './../cars/cars.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarDetailsComponent } from './components/car-details/car-details.component';
import { MatCardModule } from '@angular/material/card';
import { CarsDetailsRoutingModule } from './car-details-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    CarDetailsComponent
  ],
  imports: [
    // library modules
    MatCardModule,
    MatButtonModule,
    CommonModule,
    NgbModule,

    // app modules
    CarsDetailsRoutingModule,
    CarsModule,
    SharedModule
  ],
  exports: [
    CarsDetailsRoutingModule
  ]
})
export class CarDetailsModule { }
