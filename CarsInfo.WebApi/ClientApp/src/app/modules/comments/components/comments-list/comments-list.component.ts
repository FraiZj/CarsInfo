import { Observable } from 'rxjs';
import { selectComments } from './../../store/selectors/comments.selectors';
import { Store } from '@ngrx/store';
import { Component, Input, OnInit } from '@angular/core';
import { fetchComments } from '../../store/actions/comments.actions';
import { CommentViewModel } from '../../interfaces/comment';

@Component({
  selector: 'comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.scss']
})
export class CommentsListComponent implements OnInit {
  @Input() public carId!: number;
  public comments$: Observable<CommentViewModel[]> = this.store.select(selectComments);

  constructor(
    private readonly store: Store
  ) { }

  public ngOnInit(): void {
    this.store.dispatch(fetchComments({ carId: this.carId }));
  }

  public trackBy(index: number, comment: CommentViewModel) {
    return comment.id;
  }
}
