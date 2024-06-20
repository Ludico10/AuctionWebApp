export class CommentInfo {
  constructor(
    public userId: number,
    public lotId: number,
    public time: Date,
    public userName: string = "",
    public text: string = ""
  ) { }
}
