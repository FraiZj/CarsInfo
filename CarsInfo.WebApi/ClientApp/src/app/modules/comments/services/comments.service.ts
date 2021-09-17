import { ErrorResponse } from '@core/interfaces/error-response';
import { CommentEditor } from './../interfaces/comment-editor';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { tap } from 'rxjs/operators';
import { CommentViewModel } from '../interfaces/comment';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient
  ) { }

  public getComments(carId: number): Observable<CommentViewModel[]> {
    return this.http.get<CommentViewModel[]>(`${this.url}/cars/${carId}/comments`).pipe(
      tap({
        error: (error: ErrorResponse) => console.error(error)
      })
    );
  }

  public addComment(carId: number, comment: CommentEditor): Observable<CommentEditor> {
    return this.http.post<CommentEditor>(`${this.url}/cars/${carId}/comments`, comment).pipe(
      tap({
        error: (error: ErrorResponse) => console.error(error)
      })
    );
  }
}