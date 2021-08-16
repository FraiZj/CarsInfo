import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Car } from 'app/modules/cars/interfaces/car';

@Component({
  selector: 'car-card',
  templateUrl: './car-card.component.html',
  styleUrls: ['./car-card.component.scss']
})
export class CarCardComponent {
  @Input() public car!: Car;

  constructor(private readonly router: Router) { }

  public navigateToDetails(id: number): void {
    this.router.navigateByUrl(`cars/${id}`);
  }
}
