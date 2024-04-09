export class SimulationUser {
  constructor(
    public Id: number,
    public Name: string,
    public EstimatedCost: number,
    public Buget: number,
    public BetProbabilityBefore: number,
    public BetProbabilityAfter: number,
    public Q: number
  ) { }
}
