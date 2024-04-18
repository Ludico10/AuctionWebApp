import { HttpClient } from "@angular/common/http";
import { SimulationInfo } from "./simulationInfo";
import { Injectable } from "@angular/core";
import { SimulationResult } from "./simulationResult";

@Injectable()
export class SimulationService {

  private url = "https://localhost:7183/simulation";

  constructor(private http: HttpClient) { }

  runSimulation(info: SimulationInfo) {
    return this.http.post<SimulationResult>(this.url, info);
  }
}
