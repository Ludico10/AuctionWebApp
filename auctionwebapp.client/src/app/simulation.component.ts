import { Component, OnInit } from "@angular/core";
import { SimulationService } from "./simulation.service";
import { SimulationInfo } from "./simulationInfo";
import { SimulationBidInfo } from "./simulationBidInfo";
import { IdNameType } from "./id-name";
import { DataService } from "./data.service";

@Component({
  templateUrl: './simulation.component.html',
  providers: [SimulationService]
})

export class SimulationComponent implements OnInit {

  auctionTypes?: Object;
  auctionTypesKeys?: string[];
  info: SimulationInfo = new SimulationInfo();
  bids?: SimulationBidInfo[];

  constructor(private simulationService: SimulationService, private dataService: DataService) {
  }

  ngOnInit() {
    this.dataService.getAuctionTypes().subscribe((data: Object) => this.auctionTypes = data);
    this.auctionTypesKeys = Object.keys(this.auctionTypes!);
  }

  run() {
    this.simulationService.runSimulation(this.info).subscribe();
  }
}
