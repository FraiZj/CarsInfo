import { AuthService } from '../../../auth/services/auth.service';
import { Component, Output, EventEmitter, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormGroup, ValidationErrors } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', './../../auth-dialog.module.scss']
})
export class LoginComponent implements OnDestroy {
  @Output() public switchToRegisterEvent = new EventEmitter();
  @Output() public onLoginEvent = new EventEmitter();
  private readonly subscriptions: Subscription[] = [];
  public loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  public validationErrors: string[] = [];

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService
  ) { }

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

    this.subscriptions.push(
      this.authService.login(this.loginForm.value)
      .subscribe(() => this.onLoginEvent.emit())
    )
  }

  public switchToRegister(): void {
    this.switchToRegisterEvent.emit();
  }
}
