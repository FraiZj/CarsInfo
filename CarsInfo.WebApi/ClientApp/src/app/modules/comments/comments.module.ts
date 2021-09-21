import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentsListComponent } from './components/comments-list/comments-list.component';
import { CommentsEffects } from './store/effects/comments.effects';
import { CommentCardComponent } from './components/comment-card/comment-card.component';
import { CommentFormComponent } from './components/comment-form/comment-form.component';
import { MatInputModule } from '@angular/material/input';
import { NgxSpinnerModule } from 'ngx-spinner';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import {commentsFeatureKey} from "./store/states";
import {reducers} from "./store/reducers";

@NgModule({
  declarations: [
    CommentsListComponent,
    CommentCardComponent,
    CommentFormComponent
  ],
  imports: [
    CommonModule,
    StoreModule.forFeature({
      name: commentsFeatureKey,
      reducer: reducers
    }),
    EffectsModule.forFeature([CommentsEffects]),
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    InfiniteScrollModule,
    NgxSpinnerModule
  ],
  exports: [
    CommentsListComponent,
    CommentFormComponent
  ]
})
export class CommentsModule { }
