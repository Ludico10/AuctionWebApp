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
import { PremiumInfo } from '../model/premiumInfo';
import { UserShortInfo } from '../model/userShortInfo';
import { TrackableShortInfo } from '../model/trackableShortInfo';
import { BidShortInfo } from '../model/bidShortInfo';
import { FinishedShortInfo } from '../model/finishedShortInfo';
import { ComplaintRequest } from '../model/complaintRequest';
import { sectionShort } from '../model/sectionShort';

@Injectable()
export class DataService {

  private url: string = "http://localhost:5234/";

  public lotEdit: LotInfo | null = null;

  constructor(private http: HttpClient) { }

  login(credentials: LoginInfo) {
    return this.http.post<UserShortInfo>(this.url + "users/login", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    })
  }

  registrate(info: RegistrationInfo) {
    return this.http.post<UserShortInfo>(this.url + "users/registrate", info, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    })
  }

  refreshToken(credentials: string) {
    return this.http.post<TokenApiModel>(this.url + "users/refresh", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
  }

  getRoleName(roleId: number) {
    return this.http.get(this.url + "users/roles/" + roleId.toString());
  }

  getFreeDays(categoryId: number, year: number, month: number) {
    return this.http.get<Array<number>>(this.url + "lots/freeDays?catId=" + categoryId + "&year=" + year + "&month=" + month);
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

  getCategoriesPremium() {
    return this.http.get<Array<PremiumInfo>>(this.url + "lists/categories/premium");
  }

  getConditions() {
    return this.http.get(this.url + "lists/conditions");
  }

  getDeliveries() {
    return this.http.get(this.url + "lists/deliveries");
  }

  getComplaintsReasons() {
    return this.http.get(this.url + "lists/complaints/reasons");
  }

  placeComlaint(request: ComplaintRequest) {
    return this.http.post(this.url + "lists/complaints", request);
  }

  getSortWays() {
    return this.http.get(this.url + "lists/sorters");
  }

  getLotsShort(catalogInfo: CatalogRequest) {
    return this.http.put<LotShort[]>(this.url + "lists/catalog", catalogInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  getTrackShort(catalogInfo: CatalogRequest) {
    return this.http.put<TrackableShortInfo[]>(this.url + "lists/catalog/tracking", catalogInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  getBidsShort(catalogInfo: CatalogRequest) {
    return this.http.put<BidShortInfo[]>(this.url + "lists/catalog/bids", catalogInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  getOwnedLots(catalogInfo: CatalogRequest) {
    return this.http.put<LotShort[]>(this.url + "lists/catalog/owned", catalogInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  getWinnedLots(catalogInfo: CatalogRequest) {
    return this.http.put<FinishedShortInfo[]>(this.url + "lists/catalog/winned", catalogInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  getClosedLots(catalogInfo: CatalogRequest) {
    return this.http.put<FinishedShortInfo[]>(this.url + "lists/catalog/closed", catalogInfo, {
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

  placeComment(comment: CommentInfo, lotId: number) {
    return this.http.post(this.url + "lots/comments", comment);
  }

  getFavorite(lotId: number, userId: number) {
    return this.http.get<boolean>(this.url + "bids/track/" + lotId + "?userId=" + userId);
  }

  placeFavorite(lotId: number, userId: number, trackable: boolean) {
    return this.http.post(this.url + "bids/track/" + lotId + "?userId=" + userId, trackable);
  }

  placeLot(lotInfo: LotInfo) {
    return this.http.post<LotInfo>(this.url + "lots", lotInfo, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    });
  }

  removeLot(lotId: number) {
    return this.http.delete<boolean>(this.url + "lots/" + lotId.toString());
  }

  getImage(imageName: string, imageType: string): Observable<Blob> {
    return this.http.get(this.url + "images/" + imageType + "?name=" + imageName, { responseType: 'blob' });
  }

  getInfoNames() {
    return this.http.get<Array<sectionShort>>(this.url + "lists/info");
  }

  getCategoryText(id: number) {
    return this.http.get(this.url + "lists/info/" + id.toString());
  }
}
