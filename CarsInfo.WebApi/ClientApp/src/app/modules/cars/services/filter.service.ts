import { OrderBy } from './../enums/order-by';
import { Injectable } from "@angular/core";
import { CarsModule } from "../cars.module";
import { Filter } from "app/modules/cars-filter/interfaces/filter";

@Injectable({
  providedIn: CarsModule
})
export class FilterService {
  public getOrderBy(filterName: string): OrderBy | null {
    const filter = sessionStorage.getItem(`orderBy-${filterName}`);
    return filter != null ? JSON.parse(filter) : null;
  }

  public saveOrderBy(filterName: string, filter: Filter): void {
    sessionStorage.setItem(`orderBy-${filterName}`, JSON.stringify(filter));
  }

  public getFilter(filterName: string): Filter | null {
    const filter = sessionStorage.getItem(filterName);
    return filter != null ? JSON.parse(filter) : null;
  }

  public saveFilter(filterName: string, filter: Filter): void {
    sessionStorage.setItem(filterName, JSON.stringify(filter));
  }

  public clearFilter(filterName: string): void {
    sessionStorage.removeItem(filterName);
  }
}
