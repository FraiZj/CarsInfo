import { BaseDialogComponent } from './components/base-dialog/base-dialog.component';
import { AccessControlDirective } from './directives/access-control.directive';
import { CommonModule } from '@angular/common';
import { AuthModule } from './../auth/auth.module';
import { NgModule } from "@angular/core";
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    BaseDialogComponent,
    AccessControlDirective
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatIconModule,

    AuthModule
  ],
  exports: [
    BaseDialogComponent,
    AccessControlDirective
  ]
})
export class SharedModule { }
