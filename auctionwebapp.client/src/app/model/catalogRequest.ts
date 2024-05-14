export class CatalogRequest {
  constructor(
    public userId?: number,
    public searchString: string = "",
    public minPrice: number = 0,
    public maxPrice?: number,
    public selectedSorter: number = 5,
    public itemsOnPage: number = 15,
    public pageNumber: number = 1,
    public categoryId: number = 0,
    public auctionTypes?: Map<number, string>,
    public typeChecked: Array<boolean> = new Array(),
    public conditions?: Map<number, string>,
    public condChecked: Array<boolean> = new Array()
  ) {}
}
