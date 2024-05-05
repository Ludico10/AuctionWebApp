import { Component, OnInit } from "@angular/core";
import { DataService } from "../../services/data.service";
import { LotShort } from "../../model/lot-short";
import { CatalogRequest } from "../../model/catalogRequest";

@Component({
  selector: 'home-page',
  templateUrl: './home-page.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css'
  ],
  providers: [DataService]
})

export class HomePageComponent implements OnInit {

  categories: Map<number, string> = new Map();
  lots: LotShort[] = [];

  constructor(private dataService: DataService) { }

    ngOnInit(): void {
      this.getLastLots();
      this.getPopularCategories();
    }

  getPopularCategories() {
    this.dataService.getCategories(false).subscribe((data: any) => this.categories = data as typeof this.categories);
  }

  getLastLots() {
    let info: CatalogRequest = new CatalogRequest();
    info.itemsOnPage = 3;
    this.dataService.getLotsShort(info).subscribe((data: LotShort[]) => {
      this.lots = data;
    });

  }
}
