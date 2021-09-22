import {Injectable} from "@angular/core";
import {CoreModule} from "@core/core.module";
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: CoreModule
})
export class SnackBarService {
  constructor(
    private readonly _snackBar: MatSnackBar
  ) {
  }

  public success(message: string) {
    this._snackBar.open(message, 'X', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
      duration: 5000,
      panelClass: ['success-snackbar']
    });
  }

  public openSnackBar(message: string) {
    this._snackBar.open(message, 'X', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
      duration: 5000,
      panelClass: ['custom-snackbar']
    });
  }
}
