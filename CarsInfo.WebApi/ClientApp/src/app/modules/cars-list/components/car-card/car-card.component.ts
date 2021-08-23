import { CarsService } from './../../../cars/services/cars.service';
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
  public static readonly DefaultStarImagePath: string = '../../../../../assets/images/star-default.png';
  public static readonly SelectedStarImagePath: string = '../../../../../assets/images/star-selected.png';
  public currentImage: string = CarCardComponent.DefaultStarImagePath;

  constructor(
    private readonly carsService: CarsService,
    private readonly router: Router) { }

  ngOnInit(): void {
    this.currentImage = this.car.isLiked ?
      CarCardComponent.SelectedStarImagePath :
      CarCardComponent.DefaultStarImagePath;
  }

  public navigateToDetails(id: number): void {
    this.router.navigateByUrl(`cars/${id}`);
  }

  public onStarClick(): void {
    this.carsService.addToFavorite(this.car.id)
      .subscribe(() => this.toggleStarColor());
  }

  private toggleStarColor() {
    if (this.currentImage === CarCardComponent.DefaultStarImagePath) {
      this.currentImage = CarCardComponent.SelectedStarImagePath;
    } else {
      this.currentImage = CarCardComponent.DefaultStarImagePath;
    }
  }
}
