import { EffectsModule } from '@ngrx/effects';
import { reducer } from './store/reducers/car-editor.reducers';
import { carEditorFeatureKey } from './store/states/car-editor.states';
import { StoreModule } from '@ngrx/store';
import { CarEditorFormModule } from './../car-editor-form/car-editor-form.module';
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
import { CarsModule } from '../cars/cars.module';
import { CarEditorComponent } from './components/car-editor/car-editor.component';
import { CarEditorEffects } from './store/effects/car-editor.effects';

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
    StoreModule.forFeature({
      name: carEditorFeatureKey,
      reducer: reducer
    }),
    EffectsModule.forFeature([CarEditorEffects]),

    // app modules
    CarEditorFormModule,
    CarEditorRoutingModule,
    CoreModule,
    CarsModule,
    BrandsModule
  ],
  exports: [

  ]
})
export class CarEditorModule { }
