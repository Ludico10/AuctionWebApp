import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgClass } from "@angular/common";

import { DataService } from '../../services/data.service';
import { BidRequest } from '../../model/bidRequest'
import { LotInfo } from '../../model/lotInfo';
import { CommentInfo } from '../../model/commentInfo';
import { ModalService } from '../../services/modal.service';
import { TimeService } from '../../services/time.service';

@Component({
  selector: 'lot-page',
  templateUrl: './lot-detail.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-detail.component.css',
    '../registration-form/registration-form.component.css'
  ],
  providers: [DataService, TimeService]
})

export class LotDetailComponent implements OnInit {

  isSeller = false;
  isNew = false;

  size: number = 0;
  maxSize: number = 0;

  time = new Date();
  daysBefore: number = 0;
  hoursBefore: number = 0;
  minsBefore: number = 0;
  secsBefore: number = 0;
  intervalId: any;

  bid: BidRequest = new BidRequest();
  lot?: LotInfo;
  currentCost: number = 1;
  isFavorite: boolean = false;
  comments: Array<CommentInfo> = [];
  newComment = new CommentInfo(3, 0, this.time);

  images: Map<number, any> = new Map<number, any>();
  mainId: number = 0;
  comAvatars: Map<number, any> = new Map<number, any>();
  sellerAvatar: Map<number, any> = new Map<number, any>();

  constructor(private dataService: DataService, private timeService: TimeService, protected modalService: ModalService, private router: Router, activeRoute: ActivatedRoute) {
    this.bid.lotId = Number.parseInt(activeRoute.snapshot.queryParams["id"]);
    this.newComment.lotId = this.bid.lotId;
    let uidStr = localStorage.getItem('uid');
    if (uidStr) {
      this.bid.userId = Number.parseInt(uidStr);
    }
  }

  ngOnInit() {
    this.intervalId = setInterval(() => {
      this.time = new Date();
      if (this.lot) {
        let timeBefore = this.lot!.finishTime - this.time.getTime();
        this.daysBefore = Math.floor(timeBefore / 1000 / 60 / 60 / 24);
        let timeArr = this.timeService.getTimeFromMilli(timeBefore);
        this.hoursBefore = timeArr[0];
        this.minsBefore = timeArr[1];
        this.secsBefore = timeArr[2];
        if (this.secsBefore == 0) {
          this.dataService.getCurrentCost(this.bid.lotId!).subscribe((data: number) => this.currentCost = data);
        }
      }
    }, 1000);

    if (this.bid.lotId) {
      this.dataService.getLotInfo(this.bid.lotId).subscribe((data: LotInfo) => {
        this.lot = data;
        if (this.lot) {
          this.sellerCheck(this.lot.sellerId);
          this.dataService.getImage(this.lot.sellerId.toString(), "users").subscribe({
            next: (image: any) => {
              this.createImageFromBlob(this.sellerAvatar, this.lot!.sellerId, image);
            },
            error: () => { }
          });
        }
      });
      this.dataService.getCurrentCost(this.bid.lotId).subscribe((data: number) => {
        this.currentCost = data;
        this.isNew = (this.currentCost == this.lot?.initialCost);
      });
      this.loadCommetns();
      this.dataService.getFavorite(this.bid.lotId!, this.bid.userId!).subscribe((data: boolean) => this.isFavorite = data);
      for (let i = 0; i < 5; i++) {
        this.dataService.getImage(this.bid.lotId + "-" + i, "actual").subscribe({
          next: (data: any) => {
            this.createImageFromBlob(this.images, i, data);
          },
          error: () => { }
        });
      }
    }
  }

  loadCommetns() {
    if (this.bid.lotId) {
      this.dataService.getLotComments(this.bid.lotId).subscribe((data: any) => {
        this.comments = data as typeof this.comments;
        this.comments.forEach(com => {
          this.dataService.getImage(com.userId.toString(), "users").subscribe({
            next: (image: any) => {
              this.createImageFromBlob(this.comAvatars, com.userId, image);
            },
            error: () => { }
          });
        });
      });
    }
  }

  sellerCheck(id: number) {
    let idString = localStorage.getItem("uid");
    if (idString) {
      let uid = Number.parseInt(idString);
      this.isSeller = (uid == id);
      return;
    }
    this.isSeller = false;
  }

  createImageFromBlob(collection: Map<number, any>, i: number, image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      collection.set(i, reader.result);
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }

  selectImage(i: number) {
    this.mainId = i;
  }

  onMouseEnter(hoverName: HTMLElement) {
    hoverName.style.border = "1px";
    hoverName.style.borderStyle = "solid";
  }

  onMouseOut(hoverName: HTMLElement) {
    hoverName.style.border = "0px";
  }

  favoriteClick() {
    this.isFavorite = !this.isFavorite;
    this.dataService.placeFavorite(this.bid.lotId!, this.bid.userId!, this.isFavorite).subscribe();
  }

  placeBid() {
    this.bid.size = Math.floor(this.bid.size! * 100);
    if (this.bid.maxSize) {
      this.bid.maxSize = Math.floor(this.bid.maxSize * 100);
    }
    this.dataService.placeBid(this.bid.lotId!, this.bid).subscribe(() => {
      this.modalService.close();
      this.dataService.getCurrentCost(this.bid.lotId!).subscribe((data: number) => this.currentCost = data);
    });
  }

  placeComment() {
    if (this.newComment.text) {
      this.newComment.time = this.time;
      this.dataService.placeComment(this.newComment, this.bid.lotId!).subscribe(() => {
        this.loadCommetns();
      });
    }
  }

  ngOnDestroy() {
    clearInterval(this.intervalId);
  }

  save() {
    this.dataService.placeBid(2, this.bid).subscribe(() => this.router.navigateByUrl("/"));
  }

  redirectToEdit() {
    if (this.lot) {
      this.dataService.lotEdit = this.lot;
      this.router.navigate(["/create"], {
        state: {
          lotInfo: this.lot
        }
      });
    }
  }

  delete() {
    if (this.lot?.id && this.isSeller) {
      this.dataService.removeLot(this.lot.id).subscribe((data: boolean) => {
        if (data) {
          this.router.navigate([""]);
        }
      });
    }
  }
}
