import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ResetPasswordPageComponent} from './components/reset-password-page/reset-password-page.component';
import {ResetPasswordPageRoutingModule} from "./reset-password-page-routing.module";
import {MatFormFieldModule} from "@angular/material/form-field";
import {ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import { SendResetPasswordComponent } from './components/send-reset-password/send-reset-password.component';
import {EffectsModule} from "@ngrx/effects";
import {ResetPasswordEffects} from "./store/effects/reset-password.effects";

@NgModule({
  declarations: [
    ResetPasswordPageComponent,
    SendResetPasswordComponent
  ],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    EffectsModule.forFeature([ResetPasswordEffects]),

    ResetPasswordPageRoutingModule
  ]
})
export class ResetPasswordPageModule {
}
