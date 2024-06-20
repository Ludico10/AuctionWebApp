import { Component, OnInit } from "@angular/core";
import { DataService } from "../../services/data.service";
import { CatalogRequest } from "../../model/catalogRequest";
import { TrackableShortInfo } from "../../model/trackableShortInfo";
import { BidShortInfo } from "../../model/bidShortInfo";
import { FinishedShortInfo } from "../../model/finishedShortInfo";
import { LotShort } from "../../model/lot-short";

@Component({
  selector: "user-lots",
  templateUrl: './user-lots.component.html',
  styleUrls: ['./user-lots.component.css'],
  providers: [DataService]
})

export class UserLotsComponent implements OnInit {

  catalogInfo: CatalogRequest = new CatalogRequest();
  role = -1;
  list = -1;
  totalCount: number = 0;

  trackLots: Array<TrackableShortInfo> = new Array();
  bidLots: Array<BidShortInfo> = new Array();
  winnedLots: Array<LotShort> = new Array();
  ownedLots: Array<LotShort> = new Array();
  closedLots: Array<LotShort> = new Array();

  reasons: Map<number, string> = new Map<number, string>();

  constructor(private dataService: DataService) {
    let uidStr = localStorage.getItem('uid');
    if (uidStr) {
      this.catalogInfo.userId = Number.parseInt(uidStr);
    }
  }

  ngOnInit(): void {
    this.dataService.getComplaintsReasons().subscribe((data: any) => {
      this.reasons = data as typeof this.reasons;
      this.roleChange(0);
    });
  }

  roleChange(i: number) {
    if (i != this.role) {
      this.role = i;
      if (this.role == 0) {
        this.listChange(0);
      } else {
        this.listChange(3);
      }
    }
  }

  listChange(i: number) {
    if (i != this.list) {
      this.list = i;
      this.catalogInfo.pageNumber = 1;
      this.getItems();
    }
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

  getItems() {
    switch (this.list) {
      case 1: this.dataService.getBidsShort(this.catalogInfo)
        .subscribe((data: BidShortInfo[]) => {
          this.bidLots = data;
          if (this.bidLots.length > 0) {
            this.totalCount = this.bidLots[0].lotInfo.currentCost;
            this.bidLots.shift();
          }
        });
        break;
      case 2: this.dataService.getWinnedLots(this.catalogInfo)
        .subscribe((data: FinishedShortInfo[]) => {
          this.winnedLots = new Array<LotShort>();
          if (data.length > 0) {
            this.totalCount = data[0].cost;
            data.shift();
            data.forEach(lot => this.winnedLots.push(new LotShort(lot.id, lot.cost, lot.name, 0, lot.state, false)));
          }
        });
        break;
      case 3: this.dataService.getOwnedLots(this.catalogInfo)
        .subscribe((data: LotShort[]) => {
          this.ownedLots = data;
          if (this.ownedLots.length > 0) {
            this.totalCount = this.ownedLots[0].currentCost;
            this.ownedLots.shift();
          }
        });
        break;
      case 4: this.dataService.getClosedLots(this.catalogInfo)
        .subscribe((data: FinishedShortInfo[]) => {
          this.closedLots = new Array<LotShort>();
          if (data.length > 0) {
            this.totalCount = data[0].cost;
            data.shift();
            data.forEach(lot => this.closedLots.push(new LotShort(lot.id, lot.cost, lot.name, 0, lot.state, false)));
          }
        });
        break;
      default: this.dataService.getTrackShort(this.catalogInfo)
        .subscribe((data: TrackableShortInfo[]) => {
          this.trackLots = data;
          if (this.trackLots.length > 0) {
            this.totalCount = this.trackLots[0].lotInfo.currentCost;
            this.trackLots.shift();
          }
        });
      break;
    }
  }
}
