import { Component, EventEmitter, Input, Output } from "@angular/core";
import { SimulationUser } from "./simulationUser";

@Component({
  selector: "simulation-user-form",
  templateUrl: "./simulation-user-form.component.html",
  styleUrls: ['./simulation-user-form.component.css']
})

export class SimulationUserFormComponent {
  @Input() user!: SimulationUser;
  @Output() userChange = new EventEmitter<SimulationUser>();
}
