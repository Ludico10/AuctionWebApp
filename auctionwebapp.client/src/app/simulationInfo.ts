import { SimulationUser } from "./simulationUser";

export class SimulationInfo {
  constructor(
    public InitialPrice: number = 0,
    public PriceStep: number = 0,
    public CyclesCount: number = 1,
    public AuctionTypeId: string = "1",
    public Users: SimulationUser[] = []
  ) { }
}
