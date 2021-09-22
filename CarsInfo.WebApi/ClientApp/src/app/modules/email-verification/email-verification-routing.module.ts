import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {EmailVerificationComponent} from "./components/email-verification/email-verification.component";
import {EmailVerificationGuard} from "./guards/email-verification-guard";

const routes: Routes = [
  {
    path: '',
    component: EmailVerificationComponent,
    canActivate: [EmailVerificationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailVerificationRoutingModule {
}
