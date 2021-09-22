import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {EmailVerificationComponent} from './components/email-verification/email-verification.component';
import {EmailVerificationRoutingModule} from "./email-verification-routing.module";
import {AccountModule} from "@account/account.module";

@NgModule({
  declarations: [
    EmailVerificationComponent
  ],
  imports: [
    CommonModule,

    EmailVerificationRoutingModule,
    AccountModule
  ]
})
export class EmailVerificationModule {
}
