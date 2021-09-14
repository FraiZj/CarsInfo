import { CarDeleteConfirmationData } from './../../interfaces/car-delele-confirmation-data';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'car-delete-confirm-dialog',
  templateUrl: './car-delete-confirm-dialog.component.html',
  styleUrls: ['./car-delete-confirm-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarDeleteConfirmDialogComponent implements OnInit {
  private car$!: Observable<Car>;

  constructor(
    private readonly carsService: CarsService,
    private readonly snackBar: MatSnackBar,
    public readonly dialogRef: MatDialogRef<AuthDialogComponent>,
    private readonly router: Router,
    @Inject(MAT_DIALOG_DATA) public data: CarDeleteConfirmationData
  ) { }

  ngOnInit(): void {
    this.car$ = this.data.car$;
  }

  public onDelete(): void {
    this.car$.pipe(
      switchMap(car => this.carsService.deleteCar(car.id)),
      catchError(err => {
        if (err instanceof HttpErrorResponse) return throwError(err.error);
        if (err instanceof Error) return throwError(err.message);
        return throwError('An error occurred');
      }),
      tap({
        error: (err: string) => this.openSnackBar(err)
      })
    ).subscribe({
      next: () => {
        this.openSnackBar('Car successfully deleted');
        this.dialogRef.close();
        this.router.navigateByUrl('/cars');
      }
    });
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, 'X', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
      duration: 5000,
      panelClass: ['custom-snackbar']
    });
  }
}
