export class CatalogRequest {
  constructor(
    public minPrice: number = 0,
    public maxPrice?: number,
    public selectedSorter: number = 1,
    public itemsOnPage: number = 16,
    public pageNumber: number = 1,
    public categoryId: number = 0,
    public conditions: Map<number, string> = new Map(),
    public condChecked: Array<boolean> = new Array()
  ) {}
}
