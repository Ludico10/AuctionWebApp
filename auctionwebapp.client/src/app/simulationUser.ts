export class SimulationUser {
  constructor(
    public Id: number,
    public Color: string,
    public Name: string = "tester" + Id,
    public EstimatedCost: number = 0,
    public Budget: number = 0,
    public BetProbabilityBefore: number = 0,
    public BetProbabilityAfter: number = 0,
    public Q: number = 0
  ) { }
}
