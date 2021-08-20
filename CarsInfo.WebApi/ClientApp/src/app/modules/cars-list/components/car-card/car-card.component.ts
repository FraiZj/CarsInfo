import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Car } from 'app/modules/cars/interfaces/car';

@Component({
  selector: 'car-card',
  templateUrl: './car-card.component.html',
  styleUrls: ['./car-card.component.scss']
})
export class CarCardComponent implements OnInit {
  @Input() public car!: Car;

  constructor(private readonly router: Router) { }

  ngOnInit(): void {
    this.currentImage = this.car.isLiked ?
      CarCardComponent.SelectedStarImagePath :
      CarCardComponent.DefaultStarImagePath;
  }

  public navigateToDetails(id: number): void {
    this.router.navigateByUrl(`cars/${id}`);
  }
}
