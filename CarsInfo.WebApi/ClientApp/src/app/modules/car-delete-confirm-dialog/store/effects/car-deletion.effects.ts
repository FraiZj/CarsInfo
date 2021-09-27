import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {CarsService} from "@cars/services/cars.service";
import {catchError, exhaustMap, map} from "rxjs/operators";
import {handleError} from "@error-handler";
import {deleteCarById, deleteCarByIdSuccess} from "../actions/car-deletion.actions";
import {MatDialog} from "@angular/material/dialog";
import {SnackBarService} from "@core/services/snackbar.service";
import {Router} from "@angular/router";

@Injectable()
export class CarDeletionEffects {
  constructor(
    private readonly actions$: Actions,
    private readonly carsService: CarsService,
    public readonly dialog: MatDialog,
    private readonly snackBar: SnackBarService,
    private readonly router: Router
  ) {
  }

  deleteCarById$ = createEffect(() =>
    this.actions$.pipe(
      ofType(deleteCarById),
      map(action => action.id),
      exhaustMap(id =>
        this.carsService.deleteCar(id).pipe(
          map(() => {
            this.snackBar.success('Car successfully deleted');
            const deleteCarDialog = this.dialog.getDialogById('car-deletion-dialog');
            deleteCarDialog?.close();
            this.router.navigateByUrl('/cars');
            return deleteCarByIdSuccess();
          }),
          catchError(error => handleError(error))
        )
      )
    )
  );
}
