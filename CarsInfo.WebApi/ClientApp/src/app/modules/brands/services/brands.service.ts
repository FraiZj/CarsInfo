import { Brand } from '../interfaces/brand';
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
@Injectable({
  providedIn: 'root'
})
export class BrandsService {
  constructor(
    @Inject("BASE_API_URL") private url: string,
    private readonly http: HttpClient) {
      this.url += "/brands";
    }

  public getBrands(name?: string): Observable<Brand[]> {
    return this.http.get<Brand[]>(this.url, {
      params: {
        "name": name ?? ""
      }
    });
  }

  public addBrand(brandName: string): Observable<Brand> {
    return this.http.post<Brand>(this.url, {
      name: brandName
    });
  }
}
