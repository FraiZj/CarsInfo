import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarDeletionModalComponent } from './components/car-deletion-modal/car-deletion-modal.component';
import { MatButtonModule } from '@angular/material/button';



@NgModule({
  declarations: [
    CarDeletionModalComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule
  ]
})
export class CarDeletionModalModule { }
