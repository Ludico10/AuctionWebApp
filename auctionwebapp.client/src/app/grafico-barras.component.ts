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
  public Width = '1000px';

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
      this.Total = Math.max(element.Value, this.Total);
    });

    this.Width = Math.round(1000 / (this.list.length + 1)) + 'px'
    this.list.forEach(element => {
      element.Size = Math.round((element.Value * this.MaxHeight) / this.Total) + '%';
      element.Width = this.Width;
    });
  }

}
