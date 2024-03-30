import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { DataService } from './data.service';
import { BidRequest } from './bidRequest'

@Component({
  templateUrl: './lot-detail.component.html',
  providers: [DataService]
})

export class LotDetailComponent implements OnInit {

  bid: BidRequest = new BidRequest();

  constructor(private dataService: DataService, private router: Router, activeRoute: ActivatedRoute) {
    this.bid.LotId = Number.parseInt(activeRoute.snapshot.params["id"]);
  }

  ngOnInit() { }

  save() {
    this.dataService.placeBid(2, this.bid).subscribe(() => this.router.navigateByUrl("/"));
  }
}
