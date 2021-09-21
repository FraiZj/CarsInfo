import { UserRegister } from '@auth/interfaces/user-register';
import * as AuthActions from '@auth/store/actions/auth.actions';
import * as AuthSelectors from '@auth/store/selectors/auth.selectors';
import { Component, EventEmitter, Output, OnDestroy, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Store } from '@ngrx/store';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss', './../../auth-dialog.module.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnInit, OnDestroy {
  public static readonly PasswordMaxLength: number = 6;
  @Output() public switchToLoginEvent = new EventEmitter();
  @Output() public loginEvent = new EventEmitter();
  private readonly subscriptions: Subscription[] = [];
  public registerForm = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(RegisterComponent.PasswordMaxLength)]],
  });
  public error$ = this.store.select(AuthSelectors.selectAuthError);

  constructor(
    private formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly store: Store,
    private readonly _snackBar: MatSnackBar
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

  public get firstName(): FormControl {
    return this.registerForm.get('firstName') as FormControl;
  }

  public get lastName(): FormControl {
    return this.registerForm.get('lastName') as FormControl;
  }

  public get email(): FormControl {
    return this.registerForm.get('email') as FormControl;
  }

  public get password(): FormControl {
    return this.registerForm.get('password') as FormControl;
  }

  public onSubmit(): void {
    const userRegister = this.registerForm.value as UserRegister;
    this.store.dispatch(AuthActions.register({ userRegister }));
  }

  private openSnackBar(message: string): void {
    this._snackBar.open(message, 'X', {
      horizontalPosition: 'right',
      verticalPosition: 'top',
      duration: 5000,
      panelClass: ['custom-snackbar']
    });
  }

  public switchToLogin(): void {
    this.switchToLoginEvent.emit();
  }
}
