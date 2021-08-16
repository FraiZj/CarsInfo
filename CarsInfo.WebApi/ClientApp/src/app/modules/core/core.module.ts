import { AuthDialogModule } from './../auth-dialog/auth-dialog.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from './../shared/shared.module';
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
    AuthDialogModule,
    SharedModule
  ],
  exports: [
    NavComponent
  ]
})
export class CoreModule { }
