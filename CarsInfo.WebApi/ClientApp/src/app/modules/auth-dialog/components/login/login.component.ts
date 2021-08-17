import { AuthService } from '../../../auth/services/auth.service';
import { Component, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormControl, FormGroup, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', './../../auth-dialog.module.scss']
})
export class LoginComponent {
  @Output() public switchToRegisterEvent = new EventEmitter();
  @Output() public onLoginEvent = new EventEmitter();
  public loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  public validationErrors: string[] = [];

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService
  ) { }

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

    this.authService.login(this.loginForm.value)
      .subscribe(this.onLoginEvent.emit);
  }

  public switchToRegister(): void {
    this.switchToRegisterEvent.emit();
  }
}
