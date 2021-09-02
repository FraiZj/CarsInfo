import { Car } from 'app/modules/cars/interfaces/car';
import { Observable } from 'rxjs';

export interface CarDeleteConfirmationData {
  car$: Observable<Car>
}
