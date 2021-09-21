import { StoreModule } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { NavItemDirective } from './components/nav/directives/nav-item.directive';
import { AuthDialogModule } from '@auth-dialog/auth-dialog.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@shared/shared.module';
import { NgModule } from "@angular/core";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { MatListModule } from "@angular/material/list";
import { NavComponent } from './components/nav/nav.component';
import { MatIconModule } from '@angular/material/icon';
import {coreFeatureKey} from "@core/store/states/core.state";
import {reducer} from "@core/store/reducers/core.reducers";

@NgModule({
  declarations: [
    NotFoundComponent,
    NavComponent,
    NavItemDirective
  ],
  imports: [
    // library modules
    MatListModule,
    RouterModule,
    MatIconModule,
    CommonModule,
    StoreModule.forFeature({
      name: coreFeatureKey,
      reducer: reducer
    }),

    // app modules
    AuthDialogModule,
    SharedModule
  ],
  exports: [
    NavComponent
  ]
})
export class CoreModule { }
