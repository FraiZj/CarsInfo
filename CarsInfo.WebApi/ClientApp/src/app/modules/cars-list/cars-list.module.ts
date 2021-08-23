import { SharedModule } from './../shared/shared.module';
import { CarsModule } from './../cars/cars.module';
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
import { NgxSpinnerModule } from 'ngx-spinner';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { MatIconModule } from '@angular/material/icon';
import { FavoriteCarsListComponent } from './components/favorite-cars-list/favorite-cars-list.component';

@NgModule({
  declarations: [
    CarsListComponent,
    CarCardComponent,
    FavoriteCarsListComponent
  ],
  imports: [
    // library modules
    MatButtonModule,
    MatCardModule,
    RouterModule,
    CommonModule,
    MatIconModule,
    InfiniteScrollModule,
    NgxSpinnerModule,

    // app modules
    CoreModule,
    CarsFilterModule,
    CarsListRoutingModule,
    CarsModule,
    SharedModule
  ],
  exports: [
    CarsListRoutingModule
  ]
})
export class CarsListModule { }
