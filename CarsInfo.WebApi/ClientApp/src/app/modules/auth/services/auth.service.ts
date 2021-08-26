import { ClaimTypes } from './../enums/claim-types';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { JwtPayload } from "app/modules/auth/interfaces/jwt-payload";
import { UserClaims } from "app/modules/auth/interfaces/user-claims";
import { UserLogin } from "app/modules/auth/interfaces/user-login";
import { UserRegister } from "app/modules/auth/interfaces/user-register";
import jwtDecode from "jwt-decode";
import { BehaviorSubject, Observable } from "rxjs";
import { map, tap } from "rxjs/operators";
import { AuthModule } from "../auth.module";
import { AuthTokens } from '@auth/interfaces/auth-tokens';

@Injectable({
  providedIn: AuthModule
})
export class AuthService {
  private static readonly JwtToken: string = 'jwt-token';
  private currentUserTokenSubject!: BehaviorSubject<AuthTokens | null>;

  public get userClaims(): Observable<UserClaims | null> {
    return this.currentUserTokenSubject.pipe(map(value => {
      if (value == null) {
        return null;
      }

      const jwtPayload = jwtDecode<JwtPayload>(value.accessToken);
      const userClaims: UserClaims = {
        roles: this.configureRoles(jwtPayload),
        id: +jwtPayload.Id,
        email: jwtPayload[ClaimTypes.Email],
        token: value.accessToken
      };

      return userClaims;
    }));
  }

  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient) {
    const tokensString: string | null = localStorage.getItem(AuthService.JwtToken);
    if (tokensString != null) {
      const tokens: AuthTokens = JSON.parse(tokensString);
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(tokens);
    } else {
      this.currentUserTokenSubject = new BehaviorSubject<AuthTokens | null>(null);
    }
  }

  public getCurrentUserClaims(): UserClaims | null {
    if (this.currentUserTokenSubject.value == null) {
      return null;
    }

    const jwtPayload = jwtDecode<JwtPayload>(this.currentUserTokenSubject.value.accessToken);
    const userClaims: UserClaims = {
      roles: this.configureRoles(jwtPayload),
      id: +jwtPayload.Id,
      email: jwtPayload[ClaimTypes.Email],
      token: this.currentUserTokenSubject.value.accessToken
    };

    return userClaims;
  }

  private configureRoles(jwtPayload: JwtPayload) {
    return typeof jwtPayload[ClaimTypes.Role] == 'string' ?
      [jwtPayload[ClaimTypes.Role] as string] :
      jwtPayload[ClaimTypes.Role] as string[];
  }

  public register(userRegister: UserRegister): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/register`, userRegister).pipe(
      tap((tokens) => {
        localStorage.setItem(AuthService.JwtToken, JSON.stringify(tokens));
        this.currentUserTokenSubject.next(tokens);
      }));
  }

  public login(userLogin: UserLogin): Observable<AuthTokens> {
    return this.http.post<AuthTokens>(`${this.url}/login`, userLogin).pipe(
      tap((tokens) => {
        localStorage.setItem(AuthService.JwtToken, JSON.stringify(tokens));
        this.currentUserTokenSubject.next(tokens);
      }));
  }

  public logout(): void {
    localStorage.removeItem(AuthService.JwtToken);
    this.currentUserTokenSubject.next(null);
  }

  public isEmailAvailable(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}/emailAvailable/${email}`);
  }
}
