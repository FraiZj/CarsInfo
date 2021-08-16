import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'app/modules/auth/services/auth.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss', './../../auth-dialog.module.scss']
})
export class RegisterComponent {
  registerForm = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', Validators.required, Validators.email],
    password: ['', Validators.required],
  });
  @Output() switchToLoginEvent = new EventEmitter();
  @Output() onLoginEvent = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router) { }

  onSubmit() {
    this.authService.register(this.registerForm.value).subscribe(
      () => {
        this.onLoginEvent.emit();
      }
    );
  }

  switchToLogin() {
    this.switchToLoginEvent.emit();
  }
}
