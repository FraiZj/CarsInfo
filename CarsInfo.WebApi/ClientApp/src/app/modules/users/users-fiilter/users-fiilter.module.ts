import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { usersFilterFeatureKey } from './store/states/users-filter.state';
import { StoreModule } from '@ngrx/store';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersFilterComponent } from './components/users-filter/users-filter.component';
import { usersFilterReducer } from './store/reducers/users-filter.reducers';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    UsersFilterComponent
  ],
  imports: [
    CommonModule,
    StoreModule.forFeature({
      name: usersFilterFeatureKey,
      reducer: usersFilterReducer
    }),
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  exports: [
    UsersFilterComponent
  ]
})
export class UsersFiilterModule { }
