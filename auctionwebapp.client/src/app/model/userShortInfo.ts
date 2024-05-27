import { TokenApiModel } from "./tokenApiModel";

export class UserShortInfo {
  constructor(
    public id: number,
    public name: string,
    public roleId: number,
    public rating: number,
    public tokens?: TokenApiModel
  ) { }
}
