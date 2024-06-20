import { LotShort } from "./lot-short";

export class TrackableShortInfo {
  constructor(
    public lotInfo: LotShort,
    public automaticBid?: number
  ) { }
}
