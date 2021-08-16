import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "@core/auth/auth-guard";
import { Roles } from "../shared/enums/roles";
import { CarEditorComponent } from "./components/car-editor/car-editor.component";

const routes: Routes = [
  {
    path: '',
    component: CarEditorComponent,
    canActivate: [AuthGuard],
    data: {
      roles: [Roles.Admin]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CarEditorRoutingModule { }
