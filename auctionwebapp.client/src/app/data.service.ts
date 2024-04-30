import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BidRequest } from './bidRequest'
import { LotShort } from './lot-short';
import { Observable } from 'rxjs/internal/Observable';
import { Lot } from './lot';
import { CommentInfo } from './commentInfo';

@Injectable()
export class DataService {

  private url = "https://localhost:7183/lots/2";

  constructor(private http: HttpClient) { }

  placeBid(lotId: number, bid: BidRequest) {
    return this.http.post(this.url, bid);
  }

  getAuctionTypes() {
    return this.http.get("https://localhost:7183/lists/auctionTypes");
  }

  getCategories() {
    return this.http.get("https://localhost:7183/lists/categories");
  }

  getLotsShort(page: number, count: number, categoryId: number) {
    return this.http.get<LotShort[]>("https://localhost:7183/lots?pageNumber=" + page + "&itemsOnPage=" + count + "&category=" + categoryId);
  }

  getLotInfo(id: number) {
    return this.http.get<Lot>("https://localhost:7183/lots/" + id);
  }

  getCurrentCost(id: number) {
    return this.http.get<number>("https://localhost:7183/lots/bids/" + id);
  }

  getLotComments(lotId: number) {
    return this.http.get("https://localhost:7183/lots/comments/" + lotId);
  }

  placeComment(info: CommentInfo, lotId: number) {
    return this.http.post("https://localhost:7183/lots/comments/" + lotId, info);
  }

  getImage(imageName: string): Observable<Blob> {
    return this.http.get("https://localhost:7183/lots/image?name=" + imageName, { responseType: 'blob' });
  }
}
