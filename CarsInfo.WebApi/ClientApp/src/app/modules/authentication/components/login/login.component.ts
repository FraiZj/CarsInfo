import { AuthService } from '../../../shared/services/auth.service';
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', './../../authentication.module.scss']
})
export class LoginComponent {
  loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  @Output() switchToRegisterEvent = new EventEmitter();
  @Output() onLoginEvent = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router) { }

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe(
      () => {
        this.onLoginEvent.emit();
      }
    );
  }

  switchToRegister() {
    this.switchToRegisterEvent.emit();
  }
}
