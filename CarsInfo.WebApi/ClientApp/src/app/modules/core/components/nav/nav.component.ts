import { Store } from '@ngrx/store';
import { MatDialog } from '@angular/material/dialog';
import {ChangeDetectionStrategy, Component, OnInit,} from '@angular/core';
import { AuthenticationOption } from 'app/modules/auth-dialog/types/authentication-option';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import * as AuthActions from '@auth/store/actions/auth.actions';
import {MatSnackBar} from "@angular/material/snack-bar";
import {selectApplicationError} from "@core/store/selectors/core.selectors";
import {filter} from "rxjs/operators";

@Component({
  selector: 'carsInfo-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavComponent implements OnInit{
  public mobileMenuOpened: boolean = false;

  constructor(
    public readonly dialog: MatDialog,
    private readonly _snackBar: MatSnackBar,
    public readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.store.select(selectApplicationError).pipe(
      filter(error => error != null)
    ).subscribe(
      (error) => this.openSnackBar(error!)
    )
  }

  public toggleMobileMenu(): void {
    this.mobileMenuOpened = !this.mobileMenuOpened;
  }

  public onLogout(): void {
    this.store.dispatch(AuthActions.logout());
  }

  public openDialog(form: AuthenticationOption): void {
    this.dialog.open(AuthDialogComponent, {
      data: {
        form
      }
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
