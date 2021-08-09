import { UserRegister } from './../interfaces/user-register';
import { UserLogin } from './../interfaces/user-login';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { User } from "../interfaces/user";
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly url: string = "https://localhost:44369";
  private currentUserSubject!: BehaviorSubject<User>;
  public currentUser!: Observable<User>;


  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser') as string));
    this.currentUser = this.currentUserSubject.asObservable();
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
    this.currentUserSubject.complete();
  }

}
