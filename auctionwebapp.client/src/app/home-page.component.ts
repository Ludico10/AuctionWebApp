import { Component, OnInit } from "@angular/core";
import { DataService } from "./data.service";
import { LotShort } from "./lot-short";

@Component({
  selector: 'home-page',
  templateUrl: './home-page.component.html',
  styleUrls: [
    '../css/bootstrap.css',
    '../css/responsive.css',
    '../css/style.css'
  ],
  providers: [DataService]
})

export class HomePageComponent implements OnInit {

  categories: Map<number, string> = new Map();
  auctionTypes: Map<number, string> = new Map();
  lastLots: LotShort[] = [];
  lotTypes: string[] = [];

  constructor(private dataService: DataService) { }

    ngOnInit(): void {
      this.getAuctionTypes();
      this.getLastLots();
      this.getPopularCategories();
      this.lotTypes = [];
      this.lastLots.forEach(lot => {
        if (this.auctionTypes.has(lot.auctionTypeId))
          this.lotTypes.push(this.auctionTypes.get(lot.auctionTypeId)!);
        else
          this.lotTypes.push("English auction");
      });
    }

  getPopularCategories() {
    this.dataService.getCategories().subscribe((data: any) => this.categories = data as typeof this.categories);
  }

  getAuctionTypes() {
    this.dataService.getAuctionTypes().subscribe((data: any) => {
      this.auctionTypes = data as typeof this.auctionTypes;
    });
  }

  getLastLots() {
    this.dataService.getLotsShort(1, 3, 0).subscribe((data: LotShort[]) => {
      this.lastLots = data;
    });

  }
}
