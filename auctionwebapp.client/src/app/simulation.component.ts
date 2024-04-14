import { Component, Input, OnInit } from "@angular/core";
import { SimulationService } from "./simulation.service";
import { SimulationInfo } from "./simulationInfo";
import { SimulationBidInfo } from "./simulationBidInfo";
import { DataService } from "./data.service";
import { SimulationUser } from "./simulationUser";
import { GraficoModel } from "./grafico.model";

@Component({
  templateUrl: './simulation.component.html',
  providers: [DataService, SimulationService]
})

export class SimulationComponent implements OnInit {

  auctionTypes: Map<number, string> = new Map();
  info: SimulationInfo = new SimulationInfo();
  bids?: Array<SimulationBidInfo>;
  graficoInfo: GraficoModel[] = new Array<GraficoModel>();

  constructor(private simulationService: SimulationService, private dataService: DataService) {}

  ngOnInit() {
    this.dataService.getAuctionTypes().subscribe((data: any) => this.auctionTypes = data as typeof this.auctionTypes);
    this.addUser();
  }

  getRandomColor = function () {
    return {
      color: '#' + Math.floor(Math.random() * 16777215).toString()
    }
  };

  addUser() {
    let user = new SimulationUser(this.info.Users.length + 1, this.getRandomColor().color);
    this.info.Users.push(user);
  }

  run() {
    this.simulationService.runSimulation(this.info).subscribe((data: SimulationBidInfo[]) => {
      this.bids = data
      let graph = new Array<GraficoModel>();
      for (let i = 1; i <= this.info.CyclesCount; i++) {
        graph.push(new GraficoModel(i.toString()))
      }
      this.bids?.forEach(bid => {
        graph[bid.cycle - 1].Value = bid.size;
        graph[bid.cycle - 1].Color = this.info.Users[bid.simulationUserId - 1].Color;
      });
      this.graficoInfo = graph;
    });
  }
}
