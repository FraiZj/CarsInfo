import { CarsService } from 'app/modules/cars/services/cars.service';
import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { CarEditor } from 'app/modules/cars/interfaces/car-editor';
import { Router } from '@angular/router';

@Component({
  selector: 'car-creation',
  templateUrl: './car-creation.component.html',
  styleUrls: ['./car-creation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarCreationComponent implements OnDestroy {
  private readonly subscriptions: Subscription[] = [];

  constructor(
    private readonly carsService: CarsService,
    private readonly router: Router
  ) { }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onSubmit(car: CarEditor): void {
    this.subscriptions.push(
      this.carsService.addCar(car)
        .subscribe(() => this.router.navigateByUrl('/cars'))
    );
  }
}
