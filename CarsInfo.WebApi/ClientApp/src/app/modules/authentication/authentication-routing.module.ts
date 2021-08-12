import { AuthenticationDialogComponent } from './components/authentication-dialog/authentication-dialog.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RegisterComponent } from "./components/register/register.component";

const routes: Routes = [
  {
    path: 'login',
    component: AuthenticationDialogComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }
