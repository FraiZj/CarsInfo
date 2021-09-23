import {carsListFeatureKey} from './store/states/cars-list.states';
import {StoreModule} from '@ngrx/store';
import {MatSelectModule} from '@angular/material/select';
import {MatFormFieldModule} from '@angular/material/form-field';
import {CarsModule} from '@cars/cars.module';
import {CoreModule} from '@core/core.module';
import {NgModule} from "@angular/core";
import {MatCardModule} from "@angular/material/card";
import {RouterModule} from "@angular/router";
import {CarsFilterModule} from "@cars-filter/cars-filter.module";
import {CarsListRoutingModule} from "./cars-list-routing.module";
import {CarCardComponent} from "./components/car-card/car-card.component";
import {CarsListComponent} from "./components/cars-list/cars-list.component";
import {CommonModule} from '@angular/common';
import {MatButtonModule} from '@angular/material/button';
import {NgxSpinnerModule} from 'ngx-spinner';
import {InfiniteScrollModule} from 'ngx-infinite-scroll';
import {MatIconModule} from '@angular/material/icon';
import {FavoriteCarsListComponent} from './components/favorite-cars-list/favorite-cars-list.component';
import {CarsMainListComponent} from './components/cars-main-list/cars-main-list.component';
import {EffectsModule} from '@ngrx/effects';
import {CarsListEffects} from './store/effects/cars-list.effects';
import {reducer} from './store/reducers/cars-list.reducers';

@NgModule({
  declarations: [
    CarsListComponent,
    CarCardComponent,
    FavoriteCarsListComponent,
    CarsMainListComponent
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
    MatFormFieldModule,
    MatSelectModule,
    StoreModule.forFeature({
      name: carsListFeatureKey,
      reducer: reducer
    }),
    EffectsModule.forFeature([CarsListEffects]),

    // app modules
    CoreModule,
    CarsFilterModule,
    CarsListRoutingModule,
    CarsModule
  ],
  exports: [
    CarsListRoutingModule
  ]
})
export class CarsListModule {
}
