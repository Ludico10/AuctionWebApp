import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DataService } from './data.service';
import { Bid } from './bid'

@Component({
  templateUrl: './lot-detail.component.html',
  providers: [DataService]
})

export class LotDetailComponent implements OnInit {

  bid: Bid = new Bid();

  constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
    this.bid.BLotId = Number.parseInt(activeRoute.snapshot.params["id"]);
  }

  ngOnInit() { }

  save() {
    this.bid.BLotId = 2;
    this.dataService.placeBid(2, this.bid);
  }
}
