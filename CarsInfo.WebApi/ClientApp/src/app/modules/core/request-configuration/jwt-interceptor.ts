import { CoreModule } from './../core.module';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from 'app/modules/auth/services/auth.service';

@Injectable({
  providedIn: CoreModule
})
export class JwtInterceptor implements HttpInterceptor{
  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let currentUser = this.authService.getCurrentUserClaims();

    if( currentUser && currentUser.token){
      req = req.clone({
        setHeaders : {
          Authorization: `Bearer ` + currentUser.token
        }
      });
    }

    return next.handle(req);
  }
}

