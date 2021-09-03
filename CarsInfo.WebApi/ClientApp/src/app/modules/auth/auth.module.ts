import { CommonModule } from '@angular/common';
import * as fromAuth from './store/reducers/auth.reducer';
import { NgModule } from "@angular/core";
import { StoreModule } from '@ngrx/store';

@NgModule({
  declarations: [
  ],
  imports: [
    // library modules
    CommonModule,
    StoreModule.forFeature({
      name: fromAuth.authStatusFeatureKey,
      reducer: fromAuth.reducers,
    })
  ],
  exports: [
  ]
})
export class AuthModule { }
