export class CatalogRequest {
  constructor(
    public searchString: string = "",
    public minPrice: number = 0,
    public maxPrice?: number,
    public selectedSorter: number = 5,
    public itemsOnPage: number = 16,
    public pageNumber: number = 1,
    public categoryId: number = 0,
    public conditions?: Map<number, string>,
    public condChecked: Array<boolean> = new Array()
  ) {}
}
