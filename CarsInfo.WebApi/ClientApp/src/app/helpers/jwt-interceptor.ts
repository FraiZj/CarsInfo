import { AuthService } from '../modules/shared/services/auth.service';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor{
  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let currentUser = this.authService.getCurrentUserValue();

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

