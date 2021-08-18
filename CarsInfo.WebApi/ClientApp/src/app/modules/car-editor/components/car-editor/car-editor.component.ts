import { Observable, Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { CarsService } from 'app/modules/cars/services/cars.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CarEditor } from 'app/modules/cars/interfaces/car-editor';

@Component({
  selector: 'car-editor',
  templateUrl: './car-editor.component.html',
  styleUrls: ['./car-editor.component.scss']
})
export class CarEditorComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  private id!: number;
  public carEditor$!: Observable<CarEditor>;

  constructor(
    private readonly carsService: CarsService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) { }

  public ngOnInit(): void {
    this.id = this.getIdFromRoute();
    this.carEditor$ = this.carsService.getCarEditorById(this.id);
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onSubmit(car: CarEditor): void {
    this.subscriptions.push(
      this.carsService.updateCar(this.id, car)
        .subscribe(() => this.router.navigateByUrl(`/cars/${this.id}`))
    );
  }

  private getIdFromRoute(): number {
    const routeId = this.route.snapshot.paramMap.get('id');

    if (routeId === null) {
      this.router.navigateByUrl("404");
    }

    return parseInt(routeId as string);
  }
}
