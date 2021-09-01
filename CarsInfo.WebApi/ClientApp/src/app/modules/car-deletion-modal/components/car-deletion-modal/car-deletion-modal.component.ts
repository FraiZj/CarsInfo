import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { throwError } from 'rxjs';
import { catchError, tap, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-car-deletion-modal',
  templateUrl: './car-deletion-modal.component.html',
  styleUrls: ['./car-deletion-modal.component.scss']
})
export class CarDeletionModalComponent {
  @Input() car$!: Observable<Car>;

  constructor(
    public activeModal: NgbActiveModal,
    private readonly carsService: CarsService,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router) { }

  public onDelete(): void {
    this.car$.pipe(
      switchMap(car => this.carsService.deleteCar(car.id)),
      catchError(err => {
        if (err instanceof HttpErrorResponse) return throwError(err.error);
        if (err instanceof Error) return throwError(err.message);
        return throwError('An error occurred');
      }),
      tap({
        error: (err: string) => {
          this.snackBar.open(err, 'X', {
            horizontalPosition: 'right',
            verticalPosition: 'top',
            duration: 5000,
            panelClass: ['custom-snackbar']
          });
        }
      }))
      .subscribe({
        next: () => {
          this.snackBar.open('Car successfully deleted', 'X', {
            horizontalPosition: 'right',
            verticalPosition: 'top',
            duration: 5000,
            panelClass: ['custom-snackbar']
          });
          this.activeModal.close('delete');
          this.router.navigateByUrl('/cars');
        }
      })
  }
}
