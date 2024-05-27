import { Component, OnInit } from "@angular/core";
import { SimulationService } from "../../services/simulation.service";
import { SimulationInfo } from "../../model/simulationInfo";
import { DataService } from "../../services/data.service";
import { SimulationUser } from "../../model/simulationUser";
import { GraficoModel } from "../../model/grafico.model";
import { SimulationResult } from "../../model/simulationResult";

@Component({
  selector: "simulation",
  templateUrl: './simulation.component.html',
  styleUrls: ['./simulation.component.css'],
  providers: [DataService, SimulationService]
})

export class SimulationComponent implements OnInit {

  auctionTypes: Map<number, string> = new Map();
  info: SimulationInfo = new SimulationInfo();
  result?: SimulationResult;
  graficoInfo: GraficoModel[] = new Array<GraficoModel>();

  errorString: string = '';

  constructor(private simulationService: SimulationService, private dataService: DataService) {}

  ngOnInit() {
    this.dataService.getAuctionTypes().subscribe((data: any) => this.auctionTypes = data as typeof this.auctionTypes);
    this.addUser();
  }

  getRandomColor = function () {
    return {
      color: '#' + Math.floor(Math.random() * 255).toString(16)
        + Math.floor(Math.random() * 255).toString(16)
        + Math.floor(Math.random() * 255).toString(16)
    }
  };

  addUser() {
    let user = new SimulationUser(this.info.Users.length + 1, this.getRandomColor().color);
    this.info.Users.push(user);
  }

  run() {
    this.errorString = '';
    if (this.info.CyclesCount < 1) {
      this.errorString = "Количество раундов аукциона не может быть меньше 1.";
      return;
    }

    if (this.info.InitialPrice < 0 || this.info.PriceStep < 1) {
      this.errorString = "Значение поля, содержащего цену, не может быть отрицательным. Шаг стевки не может быть меньше 1.";
      return;
    }

    this.info.Users.forEach(user => {
      if (user.Budget < 0 || user.EstimatedCost < 0) {
        this.errorString = "Значение поля, содержащего цену, не может быть отрицательным.";
        return;
      }
    });

    this.simulationService.runSimulation(this.info).subscribe((data: SimulationResult) => {
      this.result = data
      let graph = new Array<GraficoModel>();
      for (let i = 1; i <= this.info.CyclesCount; i++) {
        graph.push(new GraficoModel(i.toString()))
      }
      this.result.bids.forEach(bid => {
        graph[bid.cycle - 1].Value = bid.size;
        graph[bid.cycle - 1].Color = this.info.Users[bid.simulationUserId - 1].Color;
      });
      this.graficoInfo = graph;
    });
  }
}
