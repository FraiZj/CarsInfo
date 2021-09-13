import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { CarEditor } from 'app/modules/cars/interfaces/car-editor';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { createCar } from '../../store/actions/car-creation.actions';

@Component({
  selector: 'car-creation',
  templateUrl: './car-creation.component.html',
  styleUrls: ['./car-creation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarCreationComponent implements OnDestroy {
  private readonly subscriptions: Subscription[] = [];

  constructor(
    private readonly store: Store,
    private readonly router: Router
  ) { }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onSubmit(car: CarEditor): void {
    this.store.dispatch(createCar({ car }));
  }
}
function carCreationSucceeded(carCreationSucceeded: any) {
  throw new Error('Function not implemented.');
}

