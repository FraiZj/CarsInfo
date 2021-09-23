import {BaseDialogComponent} from './components/base-dialog/base-dialog.component';
import {CommonModule} from '@angular/common';
import {AuthModule} from '@auth/auth.module';
import {NgModule} from "@angular/core";
import {MatDialogModule} from '@angular/material/dialog';
import {MatIconModule} from '@angular/material/icon';
import {DurationPipe} from "@shared/pipes/duration.pipe";

@NgModule({
  declarations: [
    BaseDialogComponent,
    DurationPipe
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatIconModule,

    AuthModule
  ],
  exports: [
    BaseDialogComponent,
    DurationPipe
  ]
})
export class SharedModule {
}
