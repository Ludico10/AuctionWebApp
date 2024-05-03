export class BidRequest {
  constructor(
    public time?: Date,
    public userId?: number,
    public size?: number,
    public maxSize?: number,
    public lotId?: number
  ) { }
}
