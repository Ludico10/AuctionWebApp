export class ComplaintRequest {
  constructor(
    public reasonId: number = 0,
    public lotId: number = 0,
    public comment: string = ""
  ) { }
}
