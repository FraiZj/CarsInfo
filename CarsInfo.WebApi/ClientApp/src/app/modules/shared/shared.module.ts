import { AccessControlDirective } from './directives/access-control.directive';
import { CommonModule } from '@angular/common';
import { AuthModule } from './../auth/auth.module';
import { NgModule } from "@angular/core";

@NgModule({
  declarations: [
    AccessControlDirective
  ],
  imports: [
    CommonModule,

    AuthModule
  ],
  exports: [
    AccessControlDirective
  ]
})
export class SharedModule { }
