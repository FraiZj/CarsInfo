import { Observable, Subscription } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Car } from 'app/modules/cars/interfaces/car';
import { CarsService } from 'app/modules/cars/services/cars.service';

@Component({
  selector: 'card-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.scss']
})
export class CarDetailsComponent implements OnInit {
  private id!: number;
  public car$!: Observable<Car>;

  constructor(
    private readonly carsService: CarsService,
    private readonly route: ActivatedRoute,
    private readonly router: Router) { }

  public ngOnInit(): void {
    this.id = this.getIdFromRoute();
    this.car$ = this.carsService.getCarById(this.id);
  }

  private getIdFromRoute(): number {
    const routeId = this.route.snapshot.paramMap.get('id');

    if (routeId === null) {
      this.router.navigateByUrl("404");
    }

    return parseInt(routeId as string);
  }
}
