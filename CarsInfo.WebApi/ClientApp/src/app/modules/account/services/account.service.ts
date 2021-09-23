import {Inject, Injectable} from '@angular/core';
import {AccountModule} from "../account.module";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: AccountModule
})
export class AccountService {
  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient
  ) { }

  public verifyEmail(token: string): Observable<void> {
    return this.http.head<void>(`${this.url}/verify-email`, {
      params: {
        token: token
      }
    });
  }

  public sendVerificationEmail(): Observable<void> {
    return this.http.post<void>(`${this.url}/send-verification-email`, {});
  }
}
