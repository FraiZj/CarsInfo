import { MatButtonModule } from '@angular/material/button';
import { UsersListEffects } from './store/effects/users-list.effects';
import { EffectsModule } from '@ngrx/effects';
import { usersListFeatureKey } from './store/states/users-list.state';
import { StoreModule } from '@ngrx/store';
import { UsersSharedModule } from '../users-shared';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListRoutingModule } from './users-list-routing.module';
import { UsersListComponent } from './components/user-list/users-list.component';
import { usersListReducer } from './store/reducers/users-list.reducers';
import { UsersFiilterModule } from '../users-fiilter/users-fiilter.module';

@NgModule({
  declarations: [
    UsersListComponent
  ],
  imports: [
    CommonModule,
    StoreModule.forFeature({
      name: usersListFeatureKey,
      reducer: usersListReducer
    }),
    EffectsModule.forFeature([UsersListEffects]),
    MatButtonModule,

    UsersListRoutingModule,
    UsersSharedModule,
    UsersFiilterModule
  ]
})
export class UsersListModule { }
