import { CarEditor } from '../interfaces/car-editor';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Car } from "../interfaces/car";

@Injectable({
  providedIn: 'root'
})
export class CarsService {
  constructor(
    @Inject("BASE_API_URL") private readonly url: string,
    private http: HttpClient) {
      this.url += "/cars";
    }

  public getCars(brands?: string[]): Observable<Car[]> {
    const params = this.configureParams(brands);
    return this.http.get<Car[]>(this.url, {
      params: params
    });
  }

  private configureParams(brands?: string[]) {
    let params: {
      brands?: string[];
    } = {};

    if (brands !== undefined) {
      params.brands = brands;
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
