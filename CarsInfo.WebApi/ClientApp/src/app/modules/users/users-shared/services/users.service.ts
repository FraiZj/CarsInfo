import { Observable } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { User, UserEditor } from '../interfaces';
import { UsersSharedModule } from '../users-shared.module';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: UsersSharedModule
})
export class UsersService {
  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient
  ) {
    this.url += '/users';
  }

  public getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.url);
  }

  public get(email: string): Observable<User> {
    return this.http.get<User>(`${this.url}/${email}`);
  }

  public update(email: string, model: UserEditor): Observable<void> {
    return this.http.put<void>(`${this.url}/${email}`, model);
  }

  public delete(email: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${email}`);
  }
}
