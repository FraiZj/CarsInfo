import { AuthenticationModule } from './../authentication/authentication.module';
import { NgModule } from "@angular/core";
import { CoreRoutingModule } from "./core-routing.module";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { NavComponent } from "./components/nav/nav.component";
import { MatListModule } from "@angular/material/list";

@NgModule({
  declarations: [
    NotFoundComponent,
    NavComponent
  ],
  imports: [
    // library modules
    MatListModule,

    // app modules
    CoreRoutingModule,
    AuthenticationModule
  ],
  exports: [
    NavComponent,
    NotFoundComponent,
    CoreRoutingModule
  ]
})
export class CoreModule { }
