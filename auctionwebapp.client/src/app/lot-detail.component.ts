import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgClass } from "@angular/common";

import { DataService } from './data.service';
import { BidRequest } from './bidRequest'
import { Lot } from './lot';
import { CommentInfo } from './commentInfo';

@Component({
  selector: 'lot-page',
  templateUrl: './lot-detail.component.html',
  styleUrls: [
    '../css/bootstrap.css',
    '../css/responsive.css',
    '../css/style.css',
    './lot-detail.component.css'
  ],
  providers: [DataService]
})

export class LotDetailComponent implements OnInit {

  time = new Date();
  rxTime = new Date();
  intervalId: any;

  bid: BidRequest = new BidRequest();
  lot?: Lot;
  currentCost: number = 1;
  comments: Array<CommentInfo> = [];
  newComment = new CommentInfo(3, this.time);

  constructor(private dataService: DataService, private router: Router, activeRoute: ActivatedRoute) {
    this.bid.lotId = Number.parseInt(activeRoute.snapshot.params["id"]);
  }

  ngOnInit() {
    this.intervalId = setInterval(() => {
      this.time = new Date();
    }, 1000);

    if (this.bid.lotId) {
      this.dataService.getLotInfo(this.bid.lotId).subscribe((data: Lot) => this.lot = data);
      this.dataService.getCurrentCost(this.bid.lotId).subscribe((data: number) => this.currentCost = data);
      this.dataService.getLotComments(this.bid.lotId).subscribe((data: any) => this.comments = data as typeof this.comments);
    }
  }

  placeComment() {
    if (this.newComment.text) {
      this.newComment.time = this.time;
      this.dataService.placeComment(this.newComment, this.bid.lotId!).subscribe();
    }
  }

  ngOnDestroy() {
    clearInterval(this.intervalId);
  }

  save() {
    this.dataService.placeBid(2, this.bid).subscribe(() => this.router.navigateByUrl("/"));
  }
}
