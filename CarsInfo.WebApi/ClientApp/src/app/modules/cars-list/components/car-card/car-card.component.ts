import {Subscription} from 'rxjs';
import * as CarsListSelectors from './../../store/selectors/cars-list.selectors';
import * as CarsListActions from './../../store/actions/cars-list.actions';
import {Component, Input, OnInit, OnDestroy, ChangeDetectionStrategy, ChangeDetectorRef} from '@angular/core';
import {Car} from 'app/modules/cars/interfaces/car';
import {Store} from '@ngrx/store';
import {switchMap} from 'rxjs/operators';
import {selectLoggedInOnly} from '@auth/store/selectors/auth.selectors';

@Component({
  selector: 'car-card',
  templateUrl: './car-card.component.html',
  styleUrls: ['./car-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarCardComponent implements OnInit, OnDestroy {
  @Input() public car!: Car;
  private static readonly DefaultCarImage = '../../../../../assets/images/default-car.png';
  private readonly subscriptions: Subscription[] = [];
  public static readonly DefaultStarImagePath: string = '../../../../../assets/images/star-default.png';
  public static readonly SelectedStarImagePath: string = '../../../../../assets/images/star-selected.png';
  public currentImage: string = CarCardComponent.DefaultStarImagePath;

  constructor(
    private readonly store: Store,
    private readonly cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.subscriptions.push(
      this.store.pipe(selectLoggedInOnly).pipe(
        switchMap(() => this.store.select(CarsListSelectors.favoriteCarsIds))
      ).subscribe(
        ids => {
          this.currentImage = ids.includes(this.car.id) ?
            CarCardComponent.SelectedStarImagePath :
            CarCardComponent.DefaultStarImagePath;
          this.cdr.detectChanges();
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onStarClick(): void {
    this.store.dispatch(CarsListActions.toggleFavoriteCar({id: this.car.id}));
  }

  public setDefaultPicture() {
    this.car = {...this.car, carPicturesUrls: [CarCardComponent.DefaultCarImage]};
    this.cdr.detectChanges();
  }
}
