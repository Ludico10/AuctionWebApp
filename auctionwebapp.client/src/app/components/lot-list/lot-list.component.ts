import { Component, OnInit } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

import { DataService } from '../../services/data.service';
import { CatalogRequest } from '../../model/catalogRequest';
import { LotShort } from '../../model/lot-short';

@Component({
  selector: "catalog-page",
  templateUrl: './lot-list.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-list.component.css'
  ],
  providers: [DataService]
})
export class LotListComponent implements OnInit {

  catalogInfo: CatalogRequest = new CatalogRequest();
  typesTitle: string = "Вид аукциона";
  auctionTypes: Map<number, string> = new Map();
  condTitle: string = "Состояния товара";
  categories: Map<number, string> = new Map();
  sorters: Map<number, string> = new Map();
  lots: Array<LotShort> = new Array();
  totalCount: number = 0;

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.dataService.getCategories(true).subscribe((data: any) => {
      this.categories = data as typeof this.categories;
    });
    this.dataService.getConditions().subscribe((data: any) => {
      this.catalogInfo.conditions = data as typeof this.catalogInfo.conditions;
      for (let i = 0; i < Object.keys(this.catalogInfo.conditions!).length; i++) {
        this.catalogInfo.condChecked.push(true);
      }
    });
    this.dataService.getAuctionTypes().subscribe((data: any) => {
      this.catalogInfo.auctionTypes = data as typeof this.catalogInfo.auctionTypes;
      for (let i = 0; i < Object.keys(this.catalogInfo.auctionTypes!).length; i++) {
        this.catalogInfo.typeChecked.push(true);
      }
    });
    this.dataService.getSortWays().subscribe((data: any) => this.sorters = data as typeof this.sorters);
    this.getItems();
  }

  getItems() {
    this.dataService.getLotsShort(this.catalogInfo).subscribe((data: Array<LotShort>) => {
      this.lots = data;
      if (this.lots.length > 0) {
        this.totalCount = this.lots[0].currentCost;
        this.lots.shift();
      }
    });
  }

  changePage(event: any) {
    if (this.catalogInfo.itemsOnPage != event.pageSize) {
      this.catalogInfo.itemsOnPage = event.pageSize;
      this.catalogInfo.pageNumber = 1;
    } else {
      this.catalogInfo.pageNumber = event.pageIndex + 1;
    }
    this.getItems();
  }
}
