import { MatDialog } from '@angular/material/dialog';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { AuthenticationOption } from 'app/modules/auth-dialog/types/authentication-option';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';

@Component({
  selector: 'carsInfo-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    public dialog: MatDialog) { }

  public onLogout(): void {
    this.authService.logout();
    location.reload();
    this.router.navigate(['/cars']);
  }

  public openDialog(form: AuthenticationOption): void {
    this.dialog.open(AuthDialogComponent, {
      data: {
        form
      }
    });
  }
}
