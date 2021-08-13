import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationDialogComponent } from './components/authentication-dialog/authentication-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { AuthenticationRoutingModule } from './authentication-routing.module';

@NgModule({
  declarations: [
    AuthenticationDialogComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    ReactiveFormsModule,

    AuthenticationRoutingModule
  ],
  exports: [
    AuthenticationDialogComponent,
    AuthenticationRoutingModule
  ]
})
export class AuthenticationModule { }