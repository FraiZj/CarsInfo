import { CarEditorFormModule } from './../car-editor-form/car-editor-form.module';
import { CarsModule } from './../cars/cars.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarCreationRoutingModule } from './car-creation-routing.module';
import { CarCreationComponent } from './components/car-creation.component';


@NgModule({
  declarations: [
    CarCreationComponent
  ],
  imports: [
    // library modules
    CommonModule,

    // app modules
    CarCreationRoutingModule,
    CarEditorFormModule,
    CarsModule
  ],
})
export class CarCreationModule { }
