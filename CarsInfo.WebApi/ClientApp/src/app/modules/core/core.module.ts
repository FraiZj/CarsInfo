import { RouterModule } from '@angular/router';
import { SharedModule } from './../shared/shared.module';
import { AuthenticationModule } from './../authentication/authentication.module';
import { NgModule } from "@angular/core";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { MatListModule } from "@angular/material/list";
import { NavComponent } from './components/nav/nav.component';

@NgModule({
  declarations: [
    NotFoundComponent,
    NavComponent
  ],
  imports: [
    // library modules
    MatListModule,
    RouterModule,

    // app modules
    AuthenticationModule,
    SharedModule
  ],
  exports: [
    NavComponent
  ]
})
export class CoreModule { }
