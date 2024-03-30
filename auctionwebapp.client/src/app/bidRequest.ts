export class BidRequest {
  constructor(
    public Time?: Date,
    public UserId?: number,
    public Size?: number,
    public LotId?: number,
  ) { }
}
