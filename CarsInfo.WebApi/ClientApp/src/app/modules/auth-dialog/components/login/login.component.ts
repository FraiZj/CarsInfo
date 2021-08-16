import { AuthService } from '../../../auth/services/auth.service';
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

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

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService) { }

  public onSubmit(): void {
    this.authService.login(this.loginForm.value).subscribe(
      () => {
        this.onLoginEvent.emit();
      }
    );
  }

  public switchToRegister(): void {
    this.switchToRegisterEvent.emit();
  }
}
