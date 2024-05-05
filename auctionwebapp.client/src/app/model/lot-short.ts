export class LotShort {
  constructor(
    public lotId: number,
    public currentCost: number,
    public name: string,
    public auctionTypeId: number,
    public auctionTypeName: string,
    public actual: boolean
    //public categories: Map<number, string> = new Map(),
  ) { }
}
