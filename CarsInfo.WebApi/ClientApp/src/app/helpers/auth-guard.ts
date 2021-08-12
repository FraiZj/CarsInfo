import { AuthService } from '../modules/shared/services/auth.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { User } from '../modules/shared/interfaces/user';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router){ }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
    const currentUser: User = this.authService.getCurrentUserValue();

    if(currentUser && this.isInRoles(currentUser, route.data.roles)) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }

  private isInRoles(currentUser: User, roles: string[]): boolean {
    return roles.some((r : string) => currentUser.roles.includes(r));
  }
}
