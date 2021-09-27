import {MatButtonModule} from '@angular/material/button';
import {SharedModule} from '@shared/shared.module';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CarDeleteConfirmDialogComponent} from './components/car-delete-confirm-dialog/car-delete-confirm-dialog.component';
import {EffectsModule} from "@ngrx/effects";
import {CarDeletionEffects} from "./store/effects/car-deletion.effects";
import {MatDialogModule} from "@angular/material/dialog";


@NgModule({
  declarations: [
    CarDeleteConfirmDialogComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    EffectsModule.forFeature([CarDeletionEffects]),

    SharedModule
  ],
  exports: [
    CarDeleteConfirmDialogComponent
  ]
})
export class CarDeleteConfirmDialogModule {
}
