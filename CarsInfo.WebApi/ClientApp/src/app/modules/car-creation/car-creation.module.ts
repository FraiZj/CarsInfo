import { CarCreationEffects } from './store/effects/car-creation.effects';
import { EffectsModule } from '@ngrx/effects';
import { CarEditorFormModule } from './../car-editor-form/car-editor-form.module';
import { CarsModule } from './../cars/cars.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarCreationRoutingModule } from './car-creation-routing.module';
import { CarCreationComponent } from './components/car-creation/car-creation.component';


@NgModule({
  declarations: [
    CarCreationComponent
  ],
  imports: [
    // library modules
    CommonModule,
    EffectsModule.forFeature([CarCreationEffects]),

    // app modules
    CarCreationRoutingModule,
    CarEditorFormModule,
    CarsModule
  ],
})
export class CarCreationModule { }
