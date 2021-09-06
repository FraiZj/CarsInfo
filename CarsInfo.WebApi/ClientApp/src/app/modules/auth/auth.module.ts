import { EffectsModule } from '@ngrx/effects';
import { CommonModule } from '@angular/common';
import { NgModule } from "@angular/core";
import { StoreModule } from '@ngrx/store';
import { reducer } from './store/reducers/auth.reducer';
import { authFeatureKey } from './store/states/auth.state';

@NgModule({
  declarations: [
  ],
  imports: [
    // library modules
    CommonModule,
    StoreModule.forFeature({
      name: authFeatureKey,
      reducer: reducer,
    })
  ],
  exports: [
  ]
})
export class AuthModule { }
