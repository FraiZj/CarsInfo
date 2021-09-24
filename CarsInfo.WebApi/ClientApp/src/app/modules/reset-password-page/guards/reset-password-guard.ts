import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordGuard implements CanActivate {
  constructor(
    private readonly router: Router
  ) {
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (route.queryParamMap.get('token') != null) {
      return true;
    }

    this.router.navigateByUrl('/cars');
    return false;
  }
}
