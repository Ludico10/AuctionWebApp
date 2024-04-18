import { SimulationBidInfo } from "./simulationBidInfo";

export class SimulationResult {
  constructor(
    public winnerId?: number,
    public resultCost: number = 0,
    public bids: Array<SimulationBidInfo> = []
  ) { }
}
