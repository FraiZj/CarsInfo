import { CoreModule } from './../core.module';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from 'app/modules/auth/services/auth.service';
import { switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: CoreModule
})
export class JwtInterceptor implements HttpInterceptor {
  constructor(private readonly authService: AuthService) { }

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.userClaims.pipe(switchMap(claims => {
      if (claims && claims.token) {
        req = req.clone({
          setHeaders: {
            Authorization: `Bearer ` + claims.token
          }
        });
      }

      return next.handle(req);
    }))
  }
}

