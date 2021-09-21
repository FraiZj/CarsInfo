import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges
} from '@angular/core';
import {addComment} from "../../store/actions/comment-form.actions";
import {ValidationError} from "@core/interfaces/error";
import {selectCommentFormValidationErrors} from "../../store/selectors/comments.selectors";
import {Observable, Subject} from "rxjs";
import {takeUntil} from "rxjs/operators";

@Component({
  selector: 'comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentFormComponent implements OnChanges, OnDestroy {
  @Input() carId!: number;
  public commentFormGroup: FormGroup = this.fb.group({
    text: ['', [Validators.required, Validators.maxLength(150)]]
  });
  public validationErrors$: Observable<ValidationError[]> = this.store.select(selectCommentFormValidationErrors);
  private readonly unsubscribe$: Subject<void> = new Subject<void>();

  constructor(
    private readonly store: Store,
    private readonly fb: FormBuilder,
    private readonly cdr: ChangeDetectorRef
  ) {
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.configureValidationErrors();
  }

  private configureValidationErrors() {

    this.validationErrors$.pipe(
      takeUntil(this.unsubscribe$)
    ).subscribe(
      errors => {
        errors.forEach(({field, error}) => {
          let formControl = this.commentFormGroup.get(field);
          if (formControl) {
            formControl.setErrors({
              serverError: error
            });
          }
        });
        console.log(this.text.errors?.serverError)
        this.cdr.detectChanges();
      }
    );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public get text(): FormControl {
    return this.commentFormGroup.get('text') as FormControl;
  }

  public submit(): void {
    if (this.commentFormGroup.invalid) {
      alert('Form data is invalid.');
      return;
    }

    this.store.dispatch(addComment({carId: this.carId, comment: this.commentFormGroup.value}));
    this.commentFormGroup.reset();
  }
}
