import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {ResetPasswordPageComponent} from "./components/reset-password-page/reset-password-page.component";
import {SendResetPasswordComponent} from "./components/send-reset-password/send-reset-password.component";
import {ResetPasswordGuard} from "./guards/reset-password-guard";

const routes: Routes = [
  {
    path: '',
    component: ResetPasswordPageComponent,
    canActivate: [ResetPasswordGuard]
  },
  {
    path: 'email',
    component: SendResetPasswordComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResetPasswordPageRoutingModule {
}
