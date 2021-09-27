import {selectIsLoggedIn} from '@auth/store/selectors/auth.selectors';
import {BehaviorSubject, from, Subscription} from 'rxjs';
import {Router} from '@angular/router';
import * as AuthActions from '@auth/store/actions/auth.actions';
import {AuthenticationOption} from '../../types/authentication-option';
import {ChangeDetectionStrategy, Component, Inject, OnInit, OnDestroy} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {AuthenticationDialogData} from '../../interfaces/authentication-dialog-data';
import {Store} from '@ngrx/store';
import {GoogleLoginProvider, SocialAuthService} from 'angularx-social-login';

@Component({
  selector: 'auth-dialog',
  templateUrl: './auth-dialog.component.html',
  styleUrls: ['./auth-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthDialogComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  private returnUrl!: string;
  public title$: BehaviorSubject<AuthenticationOption> = new BehaviorSubject<AuthenticationOption>('Login');

  constructor(
    public readonly store: Store,
    public readonly dialogRef: MatDialogRef<AuthDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private readonly data: AuthenticationDialogData,
    private readonly router: Router,
    private readonly socialAuthService: SocialAuthService,
  ) {
  }

  public ngOnInit(): void {
    this.title$.next(this.data.form as AuthenticationOption);
    this.returnUrl = this.data.returnUrl ?? '/cars';

    this.subscriptions.push(
      this.title$.subscribe(() => {
        this.store.dispatch(AuthActions.clearLoginError());
      }),
      this.dialogRef.afterClosed().subscribe(() => {
        this.store.dispatch(AuthActions.clearLoginError());
      })
    );
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public switchToRegister(): void {
    this.title$.next('Register');
  }

  public switchToLogin(): void {
    this.title$.next('Login');
  }

  public onLogin(): void {
    this.closeDialog();
    this.subscriptions.push(
      this.store.select(selectIsLoggedIn).subscribe(
        () => this.router.navigateByUrl(this.returnUrl)
      )
    );
  }

  public loginWithGoogle(): void {
    this.subscriptions.push(
      from(this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID)).subscribe(
        user => {
          this.store.dispatch(AuthActions.loginWithGoogle({token: user.idToken}));
          this.closeDialog();
          this.router.navigateByUrl(this.returnUrl);
        }
      )
    );
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }
}
