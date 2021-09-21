import {ChangeDetectionStrategy, Component} from '@angular/core';
import {CarEditor} from 'app/modules/cars/interfaces/car-editor';
import {Store} from '@ngrx/store';
import {createCar} from '../../store/actions/car-creation.actions';
import {Observable} from "rxjs";
import {ValidationError} from "@core/interfaces/error";
import {selectCarCreationValidationErrors} from "@car-creation/store/selectors/car-creation.selectors";

@Component({
  selector: 'car-creation',
  templateUrl: './car-creation.component.html',
  styleUrls: ['./car-creation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarCreationComponent {
  public validationErrors$: Observable<ValidationError[]> = this.store.select(selectCarCreationValidationErrors);

  constructor(
    private readonly store: Store
  ) {
  }

  public onSubmit(car: CarEditor): void {
    this.store.dispatch(createCar({car}));
  }
}

