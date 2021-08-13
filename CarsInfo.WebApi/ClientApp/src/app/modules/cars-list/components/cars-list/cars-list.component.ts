import { Filter } from './../../../cars-filter/interfaces/filter';
import { Observable } from 'rxjs';
import { CarsService } from '../../../shared/services/cars.service';
import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/modules/shared/interfaces/car';

@Component({
  selector: 'app-cars-list',
  templateUrl: './cars-list.component.html',
  styleUrls: ['./cars-list.component.scss']
})
export class CarsListComponent implements OnInit {
  cars$!: Observable<Car[]>;

  constructor(private carsService: CarsService) { }

  ngOnInit(): void {
    this.getCars();
  }

  getCars(): void {
    this.cars$ = this.carsService.getCars();
  }

  filter(filter: Filter): void {
    this.cars$ = this.carsService.getCars(filter);
  }
}