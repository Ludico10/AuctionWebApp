import { DeliveryInfo } from "./deliveryInfo";
import { CategoryInfo } from "./categoryInfo";

export class LotInfo {
  constructor(
    public sellerId: number,
    public sellerName: string,
    public sellerRating: number,
    public id?: number,
    public name: string = "",
    public description: string = "",
    public finishTime: number = 0,
    public auctionTypeId: number = 1,
    public auctionTypeName: string = "",
    public conditionId: number = 1,
    public conditionName: string = "",
    public initialCost: number = 1,
    public costStep: number =  1,
    public parameters: Map<string, string> = new Map(),
    public deliveryInfos: Array<DeliveryInfo> = new Array(),
    public categoryInfos: Array<CategoryInfo> = new Array()
  ) { }
}
