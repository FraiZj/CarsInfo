import { Injectable } from "@angular/core";
import { CarsModule } from "../cars.module";
import { FilterWithPaginator } from 'app/modules/cars-list/interfaces/filterWithPaginator';

@Injectable({
  providedIn: CarsModule
})
export class FilterService {
  public getFilter(filterName: string): FilterWithPaginator {
    const filter = sessionStorage.getItem(filterName);
    return filter != null ? JSON.parse(filter) : null;
  }

  public saveFilter(filterName: string, filter: FilterWithPaginator): void {
    sessionStorage.setItem(filterName, JSON.stringify(filter));
  }

  public clearFilter(filterName: string): void {
    sessionStorage.removeItem(filterName);
  }
}
