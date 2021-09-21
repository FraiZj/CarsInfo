import {ChangeDetectionStrategy, Component, Input} from '@angular/core';
import {CommentViewModel} from '../../interfaces/comment';

@Component({
  selector: 'comment-card',
  templateUrl: './comment-card.component.html',
  styleUrls: ['./comment-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentCardComponent {
  @Input() public comment!: CommentViewModel;
}
