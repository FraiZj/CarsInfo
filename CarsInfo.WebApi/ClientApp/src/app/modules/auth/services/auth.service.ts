import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { JwtPayload } from "app/modules/shared/interfaces/jwt-payload";
import { User } from "app/modules/shared/interfaces/user";
import { UserClaims } from "app/modules/shared/interfaces/user-claims";
import { UserLogin } from "app/modules/shared/interfaces/user-login";
import { UserRegister } from "app/modules/shared/interfaces/user-register";
import jwtDecode from "jwt-decode";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { AuthModule } from "../auth.module";

@Injectable({
  providedIn: AuthModule
})
export class AuthService {
  private static readonly JwtToken: string = 'jwt-token';
  private currentUserTokenSubject!: BehaviorSubject<string | null>;

  constructor(
    @Inject("BASE_API_URL") private readonly url: string,
    private http: HttpClient) {
    const token = localStorage.getItem(AuthService.JwtToken) as string;
    this.currentUserTokenSubject = new BehaviorSubject<string | null>(token);
  }

  public getCurrentUserClaims(): UserClaims | null {
    if (this.currentUserTokenSubject.value == null) {
      return null;
    }

    const jwtPayload = jwtDecode<JwtPayload>(this.currentUserTokenSubject.value);
    const userClaims: UserClaims = {
      roles: jwtPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      id: +jwtPayload.Id,
      email: jwtPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      token: this.currentUserTokenSubject.value
    };

    return userClaims;
  }

  public register(userRegister: UserRegister): Observable<User> {
    return this.http.post<User>(`${this.url}/register`, userRegister).pipe(
      map((user) => {
        localStorage.setItem(AuthService.JwtToken, user.token);
        this.currentUserTokenSubject.next(user.token);
        return user;
      }
    ));
  }

  public login(userLogin: UserLogin): Observable<User> {
    return this.http.post<User>(`${this.url}/login`, userLogin).pipe(
      map((user) => {
        localStorage.setItem(AuthService.JwtToken, user.token);
        this.currentUserTokenSubject.next(user.token);
        return user;
      }
    ));
  }

  public logout(): void {
    localStorage.removeItem(AuthService.JwtToken);
    this.currentUserTokenSubject.next(null);
  }
}
