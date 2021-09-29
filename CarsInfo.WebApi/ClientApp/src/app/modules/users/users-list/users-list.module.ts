import { UsersListEffects } from './store/effects/users-list.effects';
import { EffectsModule } from '@ngrx/effects';
import { usersListFeatureKey } from './store/states/users-list.state';
import { StoreModule } from '@ngrx/store';
import { UsersSharedModule } from '../users-shared';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListRoutingModule } from './users-list-routing.module';
import { UsersListComponent } from './components/user-list/users-list.component';
import { UserCardComponent } from './components/user-card/user-card.component';
import { usersListreducer } from './store/reducers/users-list.reducers';


@NgModule({
  declarations: [
    UsersListComponent,
    UserCardComponent
  ],
  imports: [
    CommonModule,
    StoreModule.forFeature({
      name: usersListFeatureKey,
      reducer: usersListreducer
    }),
    EffectsModule.forFeature([UsersListEffects]),

    UsersListRoutingModule,
    UsersSharedModule
  ]
})
export class UsersListModule { }
