import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'card-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.scss']
})
export class CarDetailsComponent implements OnInit {
  public car$!: Observable<Car>;

  constructor(
    private readonly carsService: CarsService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly snackBar: MatSnackBar) { }

  public ngOnInit(): void {
    this.car$ = this.getIdFromRoute()
      .pipe(
        switchMap(id => this.carsService.getCarById(id as number)),
        catchError(err => {
          if (err instanceof HttpErrorResponse) return throwError(err.error);
          if (err instanceof Error) return throwError(err.message);
          return throwError('An error occurred');
        }),
        tap({
          error: () => this.router.navigateByUrl("not-found")
        })
      )
  }

  public onDelete() {
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
          this.router.navigateByUrl('/cars');
        },
        error: () => {
          this.snackBar.open('An error occured while deleting car', 'X', {
            horizontalPosition: 'right',
            verticalPosition: 'top',
            duration: 5000,
            panelClass: ['custom-snackbar']
          });
        }
      })
  }

  private getIdFromRoute(): Observable<number | null> {
    return this.route.params.pipe(map(params => {
      if (params.id == null || Number.isNaN(+params.id)) {
        throw new Error(`Invalid route param id=${params.id}`);
      }

      return params.id as number;
    }))
  }
}
