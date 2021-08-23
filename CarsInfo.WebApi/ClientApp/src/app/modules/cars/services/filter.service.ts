import { Injectable } from "@angular/core";
import { CarsModule } from "../cars.module";
import { FilterWithPaginator } from 'app/modules/cars-list/interfaces/filterWithPaginator';

@Injectable({
  providedIn: CarsModule
})
export class FilterService {
  public getFilter(): FilterWithPaginator {
    const filter = sessionStorage.getItem('filter');
    return filter != null ? JSON.parse(filter) : null;
  }

  public saveFilter(filter: FilterWithPaginator): void {
    sessionStorage.setItem('filter', JSON.stringify(filter));
  }

  public clearFilter(): void {
    sessionStorage.removeItem('filter');
  }
}
