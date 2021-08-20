import { CarsModule } from './../cars.module';
import { CarEditor } from '../interfaces/car-editor';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Car } from "../interfaces/car";
import { FilterWithPaginator } from '../../cars-list/interfaces/filterWithPaginator';

@Injectable({
  providedIn: CarsModule
})
export class CarsService {
  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient) {
      this.url += "/cars";
    }

  public getCars(filter?: FilterWithPaginator): Observable<Car[]> {
    const params = this.configureParams(filter);
    return this.http.get<Car[]>(this.url, {
      params: params
    });
  }

  public getUserFavoriteCars(filter?: FilterWithPaginator): Observable<Car[]> {
    const params = this.configureParams(filter);
    return this.http.get<Car[]>(`${this.url}/favorite`, {
      params: params
    });
  }

  private configureParams(filter?: FilterWithPaginator) {
    let params: {
      brands?: string[];
      model?: string;
      skip: number;
      take: number;
    } = {
      skip: 0,
      take: 6
    };

    if (filter?.brands !== undefined) {
      params.brands = filter.brands;
    }

    if (filter?.model !== undefined && filter.model !== null && filter.model !== '') {
      params.model = filter.model;
    }

    params.skip = filter?.skip ?? 0;
    params.take = filter?.take ?? 6;

    return params;
  }

  public getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.url}/${id}`);
  }

  public getCarEditorById(id: number): Observable<CarEditor> {
    return this.http.get<CarEditor>(`${this.url}/${id}/editor`);
  }

  public addCar(car: CarEditor): Observable<CarEditor> {
    return this.http.post<CarEditor>(this.url, car);
  }

  public updateCar(id: number, car: CarEditor): Observable<CarEditor> {
    return this.http.put<CarEditor>(`${this.url}/${id}`, car);
  }
}
