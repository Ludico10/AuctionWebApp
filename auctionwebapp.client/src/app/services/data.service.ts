import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { BidRequest } from '../model/bidRequest'
import { LotShort } from '../model/lot-short';
import { Observable } from 'rxjs/internal/Observable';
import { Lot } from '../model/lot';
import { CommentInfo } from '../model/commentInfo';
import { CatalogRequest } from '../model/catalogRequest';
import { TokenApiModel } from '../model/tokenApiModel';
import { LoginInfo } from '../model/loginInfo';

@Injectable()
export class DataService {

  constructor(private http: HttpClient) { }

  login(credentials: LoginInfo) {
    return this.http.post<TokenApiModel>("https://localhost:7183/users/login", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    })
  }

  refreshToken(credentials: string) {
    return this.http.post<TokenApiModel>("https://localhost:7183/users/refresh", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
  }

  placeBid(lotId: number, bid: BidRequest) {
    return this.http.post("https://localhost:7183/lots/" + lotId, bid);
  }

  getAuctionTypes() {
    return this.http.get("https://localhost:7183/lists/auctionTypes");
  }

  getCategories(withAll: boolean) {
    return this.http.get("https://localhost:7183/lists/categories?all=" + withAll);
  }

  getConditions() {
    return this.http.get("https://localhost:7183/lists/conditions");
  }

  getSortWays() {
    return this.http.get("https://localhost:7183/lists/sorters");
  }

  getLotsShort(catalogInfo: CatalogRequest) {
    return this.http.post<LotShort[]>("https://localhost:7183/lots/catalog", catalogInfo);
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
