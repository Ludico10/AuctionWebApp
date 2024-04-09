import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BidRequest } from './bidRequest'

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
}
