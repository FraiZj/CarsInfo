import { CoreModule } from './../core/core.module';
import { NgModule } from "@angular/core";
import { MatCardModule } from "@angular/material/card";
import { RouterModule } from "@angular/router";
import { CarsFilterModule } from "../cars-filter/cars-filter.module";
import { CarsListRoutingModule } from "./cars-list-routing.module";
import { CarCardComponent } from "./components/car-card/car-card.component";
import { CarsListComponent } from "./components/cars-list/cars-list.component";
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from '../shared/shared.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [
    CarsListComponent,
    CarCardComponent
  ],
  imports: [
    // library modules
    MatButtonModule,
    MatCardModule,
    RouterModule,
    CommonModule,
    InfiniteScrollModule,
    NgxSpinnerModule,

    // app modules
    CoreModule,
    CarsFilterModule,
    CarsListRoutingModule,
    SharedModule
  ],
  exports: [
    CarsListRoutingModule
  ]
})
export class CarsListModule { }
