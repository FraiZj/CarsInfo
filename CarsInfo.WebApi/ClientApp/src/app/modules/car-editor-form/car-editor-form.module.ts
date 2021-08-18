import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrandsModule } from '../brands/brands.module';
import { CarsModule } from '../cars/cars.module';
import { CoreModule } from '@core/core.module';
import { CarEditorRoutingModule } from '../car-editor/car-editor-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { CarEditorFormComponent } from './components/car-editor-form/car-editor-form.component';

@NgModule({
  declarations: [
    CarEditorFormComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    ReactiveFormsModule,

    CoreModule,
    BrandsModule
  ],
  exports: [
    CarEditorFormComponent
  ]
})
export class CarEditorFormModule { }
