import { DeliveryInfo } from "./deliveryInfo";
import { CategoryInfo } from "./categoryInfo";

export class Lot {
  constructor(
    public id: number,
    public name: string,
    public description: string,
    public sellerId: number,
    public sellerName: string,
    public sellerRating: number,
    public finishTime: number,
    public auctionTypeId: number,
    public auctionTypeName: string,
    public conditionId: number,
    public conditionName: string,
    public initialCost: number,
    public costStep: number,
    public parameters: Map<string, string | undefined> = new Map(),
    public deliveryInfos: Array<DeliveryInfo> = new Array(),
    public categoryInfos: Array<CategoryInfo> = new Array()
  ) { }
}
