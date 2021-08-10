import { Observable } from 'rxjs';
import { CarsService } from './../../services/cars.service';
import { Component, OnInit } from '@angular/core';
import { Car } from 'src/app/interfaces/car';

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
    this.cars$ = this.carsService.getCars("");
  }

  filter(value: string): void {
    this.cars$ = this.carsService.getCars(value);
  }
}
