import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { AuthenticationOption } from 'app/modules/auth-dialog/types/authentication-option';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';

@Component({
  selector: 'carsInfo-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  public mobileMenuOpened: boolean = false;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    public dialog: MatDialog) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public toggleMobileMenu(): void {
    this.mobileMenuOpened = !this.mobileMenuOpened;
  }

  public onLogout(): void {
    this.subscriptions.push(
      this.authService.logout().subscribe(() => {
        location.reload();
        this.router.navigate(['/cars']);
      }));
  }

  public openDialog(form: AuthenticationOption): void {
    this.dialog.open(AuthDialogComponent, {
      data: {
        form
      }
    });
  }
}
