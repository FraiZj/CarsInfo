import { CommonModule } from '@angular/common';
import { NavItemDirective } from './components/nav/directives/nav-item.directive';
import { AuthDialogModule } from '@auth-dialog/auth-dialog.module';
import { RouterModule } from '@angular/router';
import { NgModule } from "@angular/core";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { MatListModule } from "@angular/material/list";
import { NavComponent } from './components/nav/nav.component';
import { MatIconModule } from '@angular/material/icon';
import {AccessControlDirective} from "@core/directives/access-control.directive";
import {EffectsModule} from "@ngrx/effects";
import {CoreEffects} from "@core/store/effects/core.effects";
import {AccountModule} from "@account/account.module";

@NgModule({
  declarations: [
    NotFoundComponent,
    NavComponent,
    NavItemDirective,
    AccessControlDirective
  ],
  imports: [
    // library modules
    MatListModule,
    RouterModule,
    MatIconModule,
    CommonModule,
    EffectsModule.forFeature([CoreEffects]),

    // app modules
    AuthDialogModule,
    AccountModule
  ],
  exports: [
    NavComponent,
    AccessControlDirective
  ]
})
export class CoreModule { }
