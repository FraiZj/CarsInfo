import { CommentsModule } from '@comments/comments.module';
import { CarDetailsEffects } from './store/effects/car-details.effects';
import { EffectsModule } from '@ngrx/effects';
import { reducer } from './store/reducers/car-details.reducers';
import { carDetailsFeatureKey } from './store/states/car-details.state';
import { StoreModule } from '@ngrx/store';
import { CarsModule } from '@cars/cars.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarDetailsComponent } from './components/car-details/car-details.component';
import { MatCardModule } from '@angular/material/card';
import { CarsDetailsRoutingModule } from './car-details-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CarDeleteConfirmDialogModule } from '../car-delete-confirm-dialog/car-delete-confirm-dialog.module';
import {CoreModule} from "@core/core.module";

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
    StoreModule.forFeature({
      name: carDetailsFeatureKey,
      reducer: reducer
    }),
    EffectsModule.forFeature([CarDetailsEffects]),

    // app modules
    CoreModule,
    CommentsModule,
    CarsDetailsRoutingModule,
    CarDeleteConfirmDialogModule,
    CarsModule
  ]
})
export class CarDetailsModule { }
