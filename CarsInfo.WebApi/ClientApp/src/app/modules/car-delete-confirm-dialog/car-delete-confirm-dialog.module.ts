import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarDeleteConfirmDialogComponent } from './components/car-delete-confirm-dialog/car-delete-confirm-dialog.component';



@NgModule({
  declarations: [
    CarDeleteConfirmDialogComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,

    SharedModule
  ],
  exports: [
    CarDeleteConfirmDialogComponent
  ]
})
export class CarDeleteConfirmDialogModule { }
