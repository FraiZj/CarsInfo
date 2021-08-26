import { UserClaims } from '../../auth/interfaces/user-claims';
import { CoreModule } from './../core.module';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { AuthDialogComponent } from 'app/modules/auth-dialog/components/auth-dialog/auth-dialog.component';

@Injectable({ providedIn: CoreModule })
export class AuthGuard implements CanActivate {
  constructor(
    private readonly authService: AuthService,
    private readonly dialog: MatDialog
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let currentUserClaims: UserClaims | null = null;
    this.authService.userClaims.subscribe(claims => currentUserClaims = claims);

    if (currentUserClaims && this.isInRoles(currentUserClaims, route.data.roles)) {
      return true;
    }

    this.dialog.open(AuthDialogComponent, {
      data: {
        form: 'Login',
        returnUrl: state.url
      }
    });

    return false;
  }

  private isInRoles(currentUser: UserClaims, roles: string[]): boolean {
    return roles.some((r: string) => currentUser.roles.includes(r));
  }
}
