import { Brand } from './../interfaces/brand';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class BrandsService {
  private readonly url: string = "https://localhost:44369/brands";

  constructor(private http: HttpClient) {}

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
