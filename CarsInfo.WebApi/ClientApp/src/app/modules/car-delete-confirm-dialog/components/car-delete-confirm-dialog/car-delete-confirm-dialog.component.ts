import {CarDeleteConfirmationData} from '../../interfaces/car-delele-confirmation-data';
import {ChangeDetectionStrategy, Component, Inject, OnInit, OnDestroy} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {Car} from 'app/modules/cars/interfaces/car';
import {Subject} from 'rxjs';
import {Store} from "@ngrx/store";
import {deleteCarById} from "../../store/actions/car-deletion.actions";

@Component({
  selector: 'car-delete-confirm-dialog',
  templateUrl: './car-delete-confirm-dialog.component.html',
  styleUrls: ['./car-delete-confirm-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarDeleteConfirmDialogComponent implements OnInit, OnDestroy {
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  private car!: Car;

  constructor(
    public readonly dialogRef: MatDialogRef<CarDeleteConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private readonly data: CarDeleteConfirmationData,
    private readonly store: Store
  ) {
  }

  ngOnInit(): void {
    this.car = this.data.car;
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onDelete(): void {
    this.store.dispatch(deleteCarById({id: this.car.id}));
  }
}
