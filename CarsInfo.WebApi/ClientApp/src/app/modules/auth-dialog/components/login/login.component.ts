import {UserLogin} from '@auth/interfaces/user-login';
import * as AuthActions from '@auth/store/actions/auth.actions';
import * as AuthSelectors from '@auth/store/selectors/auth.selectors';
import {Store} from '@ngrx/store';
import {Component, Output, EventEmitter, OnDestroy, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {FormBuilder, Validators, FormControl} from '@angular/forms';
import {Subscription} from 'rxjs';
import {MatSnackBar} from '@angular/material/snack-bar';
import {filter} from 'rxjs/operators';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', './../../auth-dialog.module.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnInit, OnDestroy {
  @Output() public switchToRegisterEvent = new EventEmitter();
  @Output() public loginEvent = new EventEmitter();
  private readonly subscriptions: Subscription[] = [];
  public loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  public validationErrors: string[] = [];
  public error$ = this.store.select(AuthSelectors.selectAuthError);

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly _snackBar: MatSnackBar,
    private readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.subscriptions.push(
      this.error$.subscribe((errors) => {
        if (errors != null && errors?.length !== 0) {
          errors.forEach(error => setTimeout(() => this.openSnackBar(error), 5000));
        }
      }),

      this.store.select(AuthSelectors.selectLoggedIn).pipe(
        filter(loggedIn => loggedIn)
      ).subscribe(
        () => this.loginEvent.emit()
      )
    );
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public get email(): FormControl {
    return this.loginForm.get('email') as FormControl;
  }

  public get password(): FormControl {
    return this.loginForm.get('password') as FormControl;
  }

  public onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const userLogin = this.loginForm.value as UserLogin;
    this.store.dispatch(AuthActions.login({userLogin}));
  }

  public switchToRegister(): void {
    this.switchToRegisterEvent.emit();
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
