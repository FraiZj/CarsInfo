import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {EmailVerificationComponent} from './components/email-verification/email-verification.component';
import {EmailVerificationRoutingModule} from "./email-verification-routing.module";
import {AccountModule} from "@account/account.module";
import {EffectsModule} from "@ngrx/effects";
import {EmailVerificationEffects} from "./store/effects/email-verification.effects";
import {CoreModule} from "@core/core.module";

@NgModule({
  declarations: [
    EmailVerificationComponent
  ],
  imports: [
    CommonModule,
    EffectsModule.forFeature([EmailVerificationEffects]),

    EmailVerificationRoutingModule,
    AccountModule,
    CoreModule
  ]
})
export class EmailVerificationModule {
}
