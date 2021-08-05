import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Car } from 'src/app/interfaces/car';

@Component({
  selector: 'app-car-card',
  templateUrl: './car-card.component.html',
  styleUrls: ['./car-card.component.scss']
})
export class CarCardComponent {
  @Input() car!: Car;

  constructor(private router: Router) { }

  navigateToDetails(id: number): void {
    this.router.navigateByUrl(`cars/${id}`);
  }
}
