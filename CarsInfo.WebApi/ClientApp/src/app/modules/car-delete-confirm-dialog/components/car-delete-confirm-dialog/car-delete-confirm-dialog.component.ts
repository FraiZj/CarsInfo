import { CarDeleteConfirmationData } from '../../interfaces/car-delele-confirmation-data';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Observable, throwError, Subject } from 'rxjs';
import { catchError, switchMap, takeUntil } from 'rxjs/operators';
import {SnackBarService} from "@core/services/snackbar.service";

@Component({
  selector: 'car-delete-confirm-dialog',
  templateUrl: './car-delete-confirm-dialog.component.html',
  styleUrls: ['./car-delete-confirm-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarDeleteConfirmDialogComponent implements OnInit, OnDestroy {
  private readonly unsubscribe$: Subject<void> = new Subject<void>();
  private car$!: Observable<Car>;

  constructor(
    private readonly carsService: CarsService,
    public readonly dialogRef: MatDialogRef<AuthDialogComponent>,
    private readonly router: Router,
    @Inject(MAT_DIALOG_DATA) private readonly data: CarDeleteConfirmationData,
    private readonly snackBar: SnackBarService
  ) { }

  ngOnInit(): void {
    this.car$ = this.data.car$;
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public onDelete(): void {
    this.car$.pipe(
      switchMap(car => this.carsService.deleteCar(car.id)),
      catchError(err => {
        if (err instanceof HttpErrorResponse) return throwError(err.error);
        if (err instanceof Error) return throwError(err.message);
        return throwError('An error occurred');
      }),
      takeUntil(this.unsubscribe$)
    ).subscribe({
      next: () => {
        this.snackBar.success('Car successfully deleted');
        this.dialogRef.close();
        this.router.navigateByUrl('/cars');
      },
      error: (err: string) => this.snackBar.openSnackBar(err)
    });
  }
}
