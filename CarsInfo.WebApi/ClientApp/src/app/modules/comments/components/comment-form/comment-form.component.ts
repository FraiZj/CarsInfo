import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { addComment } from './../../store/actions/comments.actions';
import { Store } from '@ngrx/store';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.scss']
})
export class CommentFormComponent {
  @Input() carId!: number;
  public commentFormGroup: FormGroup = this.fb.group({
    text: ['', [Validators.required, Validators.maxLength(150)]]
  });

  constructor(
    private readonly store: Store,
    private readonly fb: FormBuilder
  ) { }

  public submit(): void {
    if (this.commentFormGroup.invalid) {
      alert('Form data is invalid.');
      return;
    }

    this.store.dispatch(addComment({ carId: this.carId, comment: this.commentFormGroup.value }));
    this.commentFormGroup.reset();
  }
}
