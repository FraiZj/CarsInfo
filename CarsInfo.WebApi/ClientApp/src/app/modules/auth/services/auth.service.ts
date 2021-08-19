import { ClaimTypes } from './../enums/claim-types';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { JwtPayload } from "app/modules/auth/interfaces/jwt-payload";
import { User } from "app/modules/auth/interfaces/user";
import { UserClaims } from "app/modules/auth/interfaces/user-claims";
import { UserLogin } from "app/modules/auth/interfaces/user-login";
import { UserRegister } from "app/modules/auth/interfaces/user-register";
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
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient) {
    const token = localStorage.getItem(AuthService.JwtToken) as string;
    this.currentUserTokenSubject = new BehaviorSubject<string | null>(token);
  }

  public getCurrentUserClaims(): UserClaims | null {
    if (this.currentUserTokenSubject.value == null) {
      return null;
    }

    const jwtPayload = jwtDecode<JwtPayload>(this.currentUserTokenSubject.value);
    const userClaims: UserClaims = {
      roles: jwtPayload[ClaimTypes.Role],
      id: +jwtPayload.Id,
      email: jwtPayload[ClaimTypes.Email],
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

  public isEmailAvailable(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}/emailAvailable/${email}`);
  }
}
