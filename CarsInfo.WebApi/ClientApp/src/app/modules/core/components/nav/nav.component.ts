import {Store} from '@ngrx/store';
import {MatDialog} from '@angular/material/dialog';
import {ChangeDetectionStrategy, Component} from '@angular/core';
import {AuthenticationOption} from 'app/modules/auth-dialog/types/authentication-option';
import {AuthDialogComponent} from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';
import * as AuthActions from '@auth/store/actions/auth.actions';
import {sendVerificationEmail} from '@core/store/actions/core.actions';
import {Observable} from "rxjs";
import {selectEmailNotVerified} from "@auth/store/selectors/auth.selectors";

@Component({
  selector: 'carsInfo-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavComponent {
  public mobileMenuOpened: boolean = false;
  public emailVerified$: Observable<boolean | undefined> = this.store.select(selectEmailNotVerified);

  constructor(
    private readonly dialog: MatDialog,
    private readonly store: Store
  ) {
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

  public sendVerificationEmail() {
    this.store.dispatch(sendVerificationEmail());
  }
}
