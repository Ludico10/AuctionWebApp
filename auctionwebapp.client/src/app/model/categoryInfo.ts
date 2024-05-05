export class CategoryInfo {
  constructor(
    public categoryId: number,
    public categoryName: string,
    public premiumStart: Date,
    public premiumEnd: Date
  ) { }
}
