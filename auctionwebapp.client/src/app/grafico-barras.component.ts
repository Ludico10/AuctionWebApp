import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from "@angular/core";
import { GraficoModel } from "./grafico.model";

@Component({
  selector: 'grafico-barras',
  templateUrl: './grafico-barras.component.html',
  styleUrls: ['./grafico-barras.component.css']
})
export class GraficoBarrasComponent implements OnInit {

  @Input() list!: Array<GraficoModel>
  @Output() listChange = new EventEmitter<Array<GraficoModel>>();

  public Total = 0;
  public MaxHeight = 160;

  constructor() { }

  ngOnInit(): void {
    this.MontarGrafico();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.MontarGrafico();
  }

  MontarGrafico() {
    this.Total = 0;
    this.list.forEach(element => {
      this.Total += element.Value;
    });

    this.list.forEach(element => {
      element.Size = Math.round((element.Value * this.MaxHeight) / this.Total) + '%';
    });
  }

}
