import { BrandsModule } from './../brands/brands.module';
import { CoreModule } from './../core/core.module';
import { CarEditorRoutingModule } from './car-editor-routing.module';
import { CommonModule } from '@angular/common';
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CarEditorComponent } from "./components/car-editor/car-editor.component";
import { CarsModule } from '../cars/cars.module';

@NgModule({
  declarations: [
    CarEditorComponent
  ],
  imports: [
    // library modules
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    ReactiveFormsModule,

    // app modules
    CarEditorRoutingModule,
    CoreModule,
    CarsModule,
    BrandsModule
  ],
  exports: [
    CarEditorRoutingModule
  ]
})
export class CarEditorModule { }
