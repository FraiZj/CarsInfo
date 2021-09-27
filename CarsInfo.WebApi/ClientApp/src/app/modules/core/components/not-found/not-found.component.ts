import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'not-found',
  template: '<p class="white-text">404 Not Found</p>',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotFoundComponent { }
