import { loadNextComments } from './../../store/actions/comments.actions';
import { CommentFilter } from './../../interfaces/comment-filter';
import { Observable } from 'rxjs';
import { selectComments, selectCanLoadNextComments } from './../../store/selectors/comments.selectors';
import { Store } from '@ngrx/store';
import { Component, Input, OnInit } from '@angular/core';
import { fetchComments } from '../../store/actions/comments.actions';
import { CommentViewModel } from '../../interfaces/comment';
import { NgxSpinnerService } from 'ngx-spinner';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.scss']
})
export class CommentsListComponent implements OnInit {
  @Input() public carId!: number;
  public comments$: Observable<CommentViewModel[]> = this.store.select(selectComments);
  public filter!: CommentFilter;
  public canLoadNext = true;
  public notscrolly = true;

  constructor(
    private readonly store: Store,
    private readonly spinner: NgxSpinnerService,
    private readonly asyncPipe: AsyncPipe
  ) { }

  public ngOnInit(): void {
    this.store.dispatch(fetchComments({ carId: this.carId }));
  }

  public trackBy(index: number, comment: CommentViewModel) {
    return comment.id;
  }

  public onScroll(): void {
    this.store.select(selectCanLoadNextComments).subscribe(
      (canLoad) => {
        if (this.notscrolly && canLoad) {
          this.spinner.show();
          this.notscrolly = false;
          this.loadNextCars();
        }
      }
    );
  }

  public loadNextCars(): void {
    this.filter = {
      ...this.filter,
      skip: this.asyncPipe.transform(this.comments$)?.length ?? 0
    };

    this.spinner.hide();
    this.store.dispatch(loadNextComments({
      carId: this.carId,
      filter: this.filter
    }));
    this.notscrolly = true;
  }
}
