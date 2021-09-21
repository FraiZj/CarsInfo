import {FormGroup, FormBuilder, Validators, FormControl} from '@angular/forms';
import { Store } from '@ngrx/store';
import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {addComment} from "../../store/actions/comment-form.actions";
import {ValidationError} from "@core/interfaces/error";
import {selectCommentFormValidationErrors} from "../../store/selectors/comments.selectors";
import {Observable, Subject} from "rxjs";
import {SnackBarService} from "@core/services/snackbar.service";
import {takeUntil} from "rxjs/operators";

@Component({
  selector: 'comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.scss']
})
export class CommentFormComponent implements OnInit, OnDestroy {
  @Input() carId!: number;
  public commentFormGroup: FormGroup = this.fb.group({
    comment: ['', [Validators.required, Validators.maxLength(150)]]
  });
  public validationErrors$: Observable<ValidationError[]> = this.store.select(selectCommentFormValidationErrors);
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly store: Store,
    private readonly fb: FormBuilder,
    private readonly snackBar: SnackBarService
  ) { }

  public ngOnInit(): void {
    this.validationErrors$.pipe(
      takeUntil(this.unsubscribe$)
    ).subscribe(
      errors => errors.forEach(error => this.snackBar.openSnackBar(`${error.field} ${error.error}`))
    );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public get comment(): FormControl {
    return this.commentFormGroup.get('comment') as FormControl;
  }

  public submit(): void {
    if (this.commentFormGroup.invalid) {
      alert('Form data is invalid.');
      return;
    }

    this.store.dispatch(addComment({ carId: this.carId, comment: this.commentFormGroup.value }));
    this.commentFormGroup.reset();
  }
}
