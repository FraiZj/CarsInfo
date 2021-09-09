import { ItemsSkipPerLoad, ItemsTakePerLoad } from './../consts/filter-consts';
import { Filter } from '@cars-filter/interfaces/filter';

export class FilterWithPaginator implements Filter {
  public brands?: string[];
  public model?: string;
  public skip: number = ItemsSkipPerLoad;
  public take: number = ItemsTakePerLoad;

  public static CreateDefault(): FilterWithPaginator {
    const filter = new FilterWithPaginator();
    filter.skip = ItemsSkipPerLoad;
    filter.take = ItemsTakePerLoad;
    return filter;
  }

  public isDefault?(): boolean {
    return this.skip == ItemsSkipPerLoad &&
      this.take == ItemsTakePerLoad &&
      (this.brands?.length == 0 ?? true) &&
      (this.model?.length == 0 ?? true);
  }

  public static convertToFilter(filter: FilterWithPaginator): Filter
  {
    return {
      brands: filter.brands,
      model: filter.model
    };
  }
}
