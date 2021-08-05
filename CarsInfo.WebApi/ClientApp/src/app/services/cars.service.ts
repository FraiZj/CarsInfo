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

  public getCars(): Observable<Car[]> {
    return this.http.get<Car[]>(this.url);
  }

  public getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.url}/${id}`);
  }
}
