import {UserLogin} from '@auth/interfaces/user-login';
import * as AuthActions from '@auth/store/actions/auth.actions';
import * as AuthSelectors from '@auth/store/selectors/auth.selectors';
import {Store} from '@ngrx/store';
import {
  Component,
  Output,
  EventEmitter,
  OnDestroy,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef
} from '@angular/core';
import {FormBuilder, Validators, FormControl} from '@angular/forms';
import {Observable, Subscription} from 'rxjs';
import {MatSnackBar} from '@angular/material/snack-bar';
import {ValidationError} from "@core/interfaces/error";
import {Router} from "@angular/router";

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', './../../auth-dialog.module.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnInit, OnDestroy {
  @Output() public switchToRegisterEvent = new EventEmitter();
  @Output() public loginSubmit = new EventEmitter();
  @Output() public passwordReset = new EventEmitter();
  private readonly subscriptions: Subscription[] = [];
  public loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  public validationErrors$: Observable<ValidationError[]> = this.store.select(AuthSelectors.selectAuthValidationErrors);

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly _snackBar: MatSnackBar,
    private readonly store: Store,
    private readonly cdr: ChangeDetectorRef,
    private readonly router: Router
  ) {
  }

  public ngOnInit(): void {
    this.configureValidationErrors();
    this.subscriptions.push(
      this.store.pipe(AuthSelectors.selectLoggedInOnly).subscribe(
        () => this.loginSubmit.emit()
      )
    );
  }

  private configureValidationErrors() {
    this.subscriptions.push(
      this.validationErrors$.subscribe(
        errors => {
          errors.forEach(({field, error}) => {
            let formControl = this.loginForm.get(field);
            if (formControl) {
              formControl.setErrors({
                serverError: error
              });
            }
          });
          this.cdr.detectChanges();
        }
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

  public resetPassword() {
    this.passwordReset.emit();
    this.router.navigateByUrl('/reset-password/email');
  }
}
