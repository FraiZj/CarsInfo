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
  public static readonly DefaultStarImagePath: string= '../../../../../assets/images/star-default.png';
  public static readonly SelectedStarImagePath: string = '../../../../../assets/images/star-selected.png';
  public currentImage: string = CarCardComponent.DefaultStarImagePath;

  constructor(private readonly router: Router) { }

  public navigateToDetails(id: number): void {
    this.router.navigateByUrl(`cars/${id}`);
  }

  public onStarClick(): void {
    if (this.currentImage === CarCardComponent.DefaultStarImagePath) {
      this.currentImage = CarCardComponent.SelectedStarImagePath;
    } else {
      this.currentImage = CarCardComponent.DefaultStarImagePath;
    }
  }
}
