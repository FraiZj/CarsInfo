import { Subscription } from 'rxjs';
import { ToggleFavoriteStatus } from './../../../cars/enums/toggle-favorite-status';
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
  private readonly subscriptions: Subscription[] = [];
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
    this.subscriptions.push(
      this.carsService.toggleFavorite(this.car.id)
        .subscribe(status => this.toggleStarColor(status))
    );
  }

  private toggleStarColor(status: ToggleFavoriteStatus) {
    if (status === ToggleFavoriteStatus.AddedToFavorite) {
      this.currentImage = CarCardComponent.SelectedStarImagePath;
    } else {
      this.currentImage = CarCardComponent.DefaultStarImagePath;
    }
  }
}
