import { Component, OnInit } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DataService } from './data.service';
import { CatalogRequest } from './catalogRequest';

@Component({
  templateUrl: './lot-list.component.html',
  styleUrls: [
    '../css/bootstrap.css',
    '../css/responsive.css',
    '../css/style.css',
    './lot-list.component.css'
  ],
  providers: [DataService]
})
export class LotListComponent implements OnInit {

  catalogInfo: CatalogRequest = new CatalogRequest();
  condTitle: string = "Состояния товара";
  public categories: Map<number, string> = new Map();
  public sorters: Map<number, string> = new Map();

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.dataService.getCategories().subscribe((data: any) => {
      this.categories = data as typeof this.categories;
    });
    this.dataService.getConditions().subscribe((data: any) => {
      this.catalogInfo.conditions = data as typeof this.catalogInfo.conditions;
      for (let i = 0; i < Object.keys(this.catalogInfo.conditions).length; i++) {
        this.catalogInfo.condChecked.push(true);
      }
    });
    this.dataService.getSortWays().subscribe((data: any) => this.sorters = data as typeof this.sorters);
  }

  getItems() {

  }
}
