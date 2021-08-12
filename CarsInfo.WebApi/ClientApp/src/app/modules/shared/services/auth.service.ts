import { UserRegister } from '../interfaces/user-register';
import { UserLogin } from '../interfaces/user-login';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { User } from "../interfaces/user";
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject!: BehaviorSubject<User>;

  constructor(
    @Inject("BASE_API_URL") private readonly url: string,
    private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser') as string));
  }

  public getCurrentUserValue(): User {
    return this.currentUserSubject.value;
  }

  public register(userRegister: UserRegister): Observable<User> {
    return this.http.post<User>(`${this.url}/register`, userRegister).pipe(
      map((user) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user as User);
        return user;
      }
    ));
  }

  public login(userLogin: UserLogin): Observable<User> {
    return this.http.post<User>(`${this.url}/login`, userLogin).pipe(
      map((user) => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user as User);
        return user;
      }
    ));
  }

  public logout(): void {
    localStorage.removeItem('currentUser');
  }
}
