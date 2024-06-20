import { LotShort } from "./lot-short";

export class BidShortInfo {
  constructor(
    public lotInfo: LotShort,
    public size: number,
    public time: Date
  ) { }
}
