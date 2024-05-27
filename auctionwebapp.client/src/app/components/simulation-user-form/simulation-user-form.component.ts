import { Component, EventEmitter, Input, Output } from "@angular/core";
import { SimulationUser } from "../../model/simulationUser";

@Component({
  selector: "simulation-user",
  templateUrl: "./simulation-user-form.component.html",
  styleUrls: ['./simulation-user-form.component.css']
})

export class SimulationUserFormComponent {
  @Input() user!: SimulationUser;
  @Output() userChange = new EventEmitter<SimulationUser>();
}
