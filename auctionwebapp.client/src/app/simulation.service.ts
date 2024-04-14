import { HttpClient } from "@angular/common/http";
import { SimulationInfo } from "./simulationInfo";
import { Injectable } from "@angular/core";
import { SimulationBidInfo } from "./simulationBidInfo";

@Injectable()
export class SimulationService {

  private url = "https://localhost:7183/simulation";

  constructor(private http: HttpClient) { }

  runSimulation(info: SimulationInfo) {
    return this.http.post<Array<SimulationBidInfo>>(this.url, info);
  }
}
