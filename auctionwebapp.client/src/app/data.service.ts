import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { BidRequest } from './bidRequest'
import { LotShort } from './lot-short';
import { Observable } from 'rxjs/internal/Observable';
import { Lot } from './lot';
import { CommentInfo } from './commentInfo';
import { CatalogRequest } from './catalogRequest';

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

  getConditions() {
    return this.http.get("https://localhost:7183/lists/conditions");
  }

  getSortWays() {
    return this.http.get("https://localhost:7183/lists/sorters");
  }

  getLotsShort(catalogInfo: CatalogRequest) {
    return this.http.put<LotShort[]>("https://localhost:7183/lots", catalogInfo);
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

  getFavorite(lotId: number, userId: number) {
    return this.http.get<boolean>("https://localhost:7183/lots/track/" + lotId + "?userId=" + userId);
  }

  placeFavorite(lotId: number, userId: number, trackable: boolean) {
    return this.http.post("https://localhost:7183/lots/track/" + lotId + "?userId=" + userId, trackable);
  }

  placeComment(info: CommentInfo, lotId: number) {
    return this.http.post("https://localhost:7183/lots/comments/" + lotId, info);
  }

  getImage(imageName: string): Observable<Blob> {
    return this.http.get("https://localhost:7183/lots/image?name=" + imageName, { responseType: 'blob' });
  }
}
