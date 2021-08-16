import { CarsService } from '../../../shared/services/cars.service';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Car } from 'app/modules/shared/interfaces/car';

@Component({
  selector: 'app-card-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.scss']
})
export class CarDetailsComponent implements OnInit {
  id!: number;
  car$!: Observable<Car>;

  constructor(
    private carsService: CarsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.getIdFromRoute();
    this.car$ = this.carsService.getCarById(this.id);
  }

  private getIdFromRoute() : number {
    const routeId = this.route.snapshot.paramMap.get('id');

    if (routeId === null) {
      this.router.navigateByUrl("404");
    }

    return parseInt(routeId as string);
  }
}
