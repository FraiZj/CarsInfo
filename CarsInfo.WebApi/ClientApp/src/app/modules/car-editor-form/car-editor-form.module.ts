import { EffectsModule } from '@ngrx/effects';
import { reducer } from './store/reducers/car-editor-form.reducers';
import { carEditorFormFeatureKey } from './store/states/car-editor-form.state';
import { StoreModule } from '@ngrx/store';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrandsModule } from '../brands/brands.module';
import { CoreModule } from '@core/core.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { CarEditorFormComponent } from './components/car-editor-form/car-editor-form.component';
import { CarEditorFormEffects } from './store/effects/car-editor-form.effects';

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
    StoreModule.forFeature({
      name: carEditorFormFeatureKey,
      reducer: reducer
    }),
    EffectsModule.forFeature([CarEditorFormEffects]),

    CoreModule,
    BrandsModule
  ],
  exports: [
    CarEditorFormComponent
  ]
})
export class CarEditorFormModule { }
