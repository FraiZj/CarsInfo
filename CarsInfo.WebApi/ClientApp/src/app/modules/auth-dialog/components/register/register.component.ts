import {UserRegister} from '@auth/interfaces/user-register';
import * as AuthActions from '@auth/store/actions/auth.actions';
import * as AuthSelectors from '@auth/store/selectors/auth.selectors';
import {
  Component,
  EventEmitter,
  Output,
  OnDestroy,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef
} from '@angular/core';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {AuthService} from 'app/modules/auth/services/auth.service';
import {Observable, Subscription} from 'rxjs';
import {filter} from 'rxjs/operators';
import {ValidationError} from "@core/interfaces/error";

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
  public validationErrors$: Observable<ValidationError[]> = this.store.select(AuthSelectors.selectAuthValidationErrors);

  constructor(
    private formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly store: Store,
    private readonly cdr: ChangeDetectorRef
  ) {
  }

  public ngOnInit(): void {
    this.configureValidationErrors();
    this.subscriptions.push(
      this.store.select(AuthSelectors.selectLoggedIn).pipe(
        filter(loggedIn => loggedIn)
      ).subscribe(
        () => this.loginEvent.emit()
      )
    );
  }

  private configureValidationErrors() {
    this.subscriptions.push(
      this.validationErrors$.subscribe(
        errors => {
          errors.forEach(({field, error}) => {
            let formControl = this.registerForm.get(field);
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
    this.store.dispatch(AuthActions.register({userRegister}));
  }

  public switchToLogin(): void {
    this.switchToLoginEvent.emit();
  }
}
