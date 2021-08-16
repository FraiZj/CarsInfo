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
    private authService: AuthService,
    private router: Router,
    public dialog: MatDialog) { }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/cars']);
  }

  openDialog(form: AuthenticationOption): void {
    const dialogRef = this.dialog.open(AuthDialogComponent, {
      data: {
        form
      }
    });

    dialogRef.afterClosed();
  }
}
