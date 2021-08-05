import { CarsService } from './../../services/cars.service';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/interfaces/car';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-card-details',
  templateUrl: './card-details.component.html',
  styleUrls: ['./card-details.component.scss']
})
export class CardDetailsComponent implements OnInit {
  id!: number;
  car$!: Observable<Car>;

  constructor(
    private carsService: CarsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.getIdFromRoute();
    this.car$ = this.carsService.getCarById(this.id);
    console.log(this.car$.subscribe(c => console.log(c)));
  }

  private getIdFromRoute() : number {
    const routeId = this.route.snapshot.paramMap.get('id');
    console.log(routeId);

    if (routeId === null) {
      this.router.navigateByUrl("404");
    }

    return parseInt(routeId as string);
  }


}
