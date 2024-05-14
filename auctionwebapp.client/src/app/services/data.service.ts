import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { BidRequest } from '../model/bidRequest'
import { LotShort } from '../model/lot-short';
import { Observable } from 'rxjs/internal/Observable';
import { LotInfo } from '../model/lotInfo';
import { CommentInfo } from '../model/commentInfo';
import { CatalogRequest } from '../model/catalogRequest';
import { TokenApiModel } from '../model/tokenApiModel';
import { LoginInfo } from '../model/loginInfo';
import { RegistrationInfo } from '../model/registrationInfo';

@Injectable()
export class DataService {

  private url: string = "http://localhost:5234/";

  constructor(private http: HttpClient) { }

  login(credentials: LoginInfo) {
    return this.http.post<TokenApiModel>(this.url + "users/login", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    })
  }

  registrate(info: RegistrationInfo) {
    return this.http.post<TokenApiModel>(this.url + "users/registrate", info, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    })
  }

  refreshToken(credentials: string) {
    return this.http.post<TokenApiModel>(this.url + "users/refresh", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
  }

  placeBid(lotId: number, bid: BidRequest) {
    return this.http.post(this.url + "bids", bid);
  }

  getAuctionTypes() {
    return this.http.get(this.url + "lists/auctionTypes");
  }

  getCategories(withAll: boolean) {
    return this.http.get(this.url + "lists/categories?all=" + withAll);
  }

  getConditions() {
    return this.http.get(this.url + "lists/conditions");
  }

  getDeliveries() {
    return this.http.get(this.url + "lists/deliveries");
  }

  getSortWays() {
    return this.http.get(this.url + "lists/sorters");
  }

  getLotsShort(catalogInfo: CatalogRequest) {
    return this.http.put<LotShort[]>(this.url + "lists/catalog", catalogInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  getLotInfo(id: number) {
    return this.http.get<LotInfo>(this.url + "lots/" + id);
  }

  getCurrentCost(id: number) {
    return this.http.get<number>(this.url + "bids/" + id);
  }

  getLotComments(lotId: number) {
    return this.http.get(this.url + "lots/comments/" + lotId);
  }

  placeComment(info: CommentInfo, lotId: number) {
    return this.http.post(this.url + "lots/comments/" + lotId, info);
  }

  getFavorite(lotId: number, userId: number) {
    return this.http.get<boolean>(this.url + "bids/track/" + lotId + "?userId=" + userId);
  }

  placeFavorite(lotId: number, userId: number, trackable: boolean) {
    return this.http.post(this.url + "bids/track/" + lotId + "?userId=" + userId, trackable);
  }

  getImage(imageName: string, imageType: string): Observable<Blob> {
    return this.http.get(this.url + "images/" + imageType + "?name=" + imageName, { responseType: 'blob' });
  }
}
