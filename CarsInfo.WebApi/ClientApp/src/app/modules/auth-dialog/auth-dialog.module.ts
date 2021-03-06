import { SharedModule } from '@shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthModule } from '@auth/auth.module';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthDialogComponent } from './components/auth-dialog/auth-dialog.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import {RouterModule} from "@angular/router";

@NgModule({
  declarations: [
    AuthDialogComponent,
    LoginComponent,
    RegisterComponent
  ],
    imports: [
        // library modules
        CommonModule,
        MatDialogModule,
        MatFormFieldModule,
        MatButtonModule,
        MatInputModule,
        MatIconModule,
        ReactiveFormsModule,
        MatSnackBarModule,
        RouterModule,

        // app modules
        AuthModule,
        SharedModule
    ],
  exports: [
    AuthDialogComponent,
  ]
})
export class AuthDialogModule { }
