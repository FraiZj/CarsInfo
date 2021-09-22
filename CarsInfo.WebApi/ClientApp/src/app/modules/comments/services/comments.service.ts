import { CommentOrderBy } from '../enums/comment-order-by';
import { CommentFilter } from '../interfaces/comment-filter';
import { ErrorResponse } from '@core/interfaces/error-response';
import { CommentEditor } from '../interfaces/comment-editor';
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

  public getComments(carId: number, filter?: CommentFilter): Observable<CommentViewModel[]> {
    const params = CommentsService.configureParams(filter);
    return this.http.get<CommentViewModel[]>(`${this.url}/cars/${carId}/comments`, {
      params: params
    });
  }

  private static configureParams(filter?: CommentFilter) {
    return {
      skip: filter?.skip ?? 0,
      take: filter?.take ?? 10,
      orderBy: filter?.orderBy ?? CommentOrderBy.PublishDateDesc
    };
  }

  public addComment(carId: number, comment: CommentEditor): Observable<CommentEditor> {
    return this.http.post<CommentEditor>(`${this.url}/cars/${carId}/comments`, comment);
  }
}
