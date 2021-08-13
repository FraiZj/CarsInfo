import { CarEditor } from '../interfaces/car-editor';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Car } from "../interfaces/car";
import { Filter } from '../../cars-filter/interfaces/filter';

@Injectable({
  providedIn: 'root'
})
export class CarsService {
  constructor(
    @Inject("BASE_API_URL") private readonly url: string,
    private http: HttpClient) {
      this.url += "/cars";
    }

  public getCars(filter?: Filter): Observable<Car[]> {
    const params = this.configureParams(filter);
    return this.http.get<Car[]>(this.url, {
      params: params
    });
  }

  private configureParams(filter?: Filter) {
    let params: {
      brands?: string[];
      model?: string
    } = {};

    if (filter?.brands !== undefined) {
      params.brands = filter.brands;
    }

    if (filter?.model !== undefined && filter.model !== null && filter.model !== '') {
      params.model = filter.model;
    }

    return params;
  }

  public getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.url}/${id}`);
  }

  public addCar(car: CarEditor) {
    return this.http.post<CarEditor>(this.url, car);
  }
}
