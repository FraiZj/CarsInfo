import { CarEditor } from './../interfaces/car-editor';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Car } from "../interfaces/car";

@Injectable({
  providedIn: 'root'
})
export class CarsService {
  private readonly url: string = "https://localhost:44369/cars";

  constructor(private http: HttpClient) {}

  public getCars(brand: string): Observable<Car[]> {
    return this.http.get<Car[]>(this.url, {
      params: {
        "brand": brand
      }
    });
  }

  public getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.url}/${id}`);
  }

  public addCar(car: CarEditor) {
    return this.http.post<CarEditor>(this.url, car);
  }
}
