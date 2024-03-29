import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Bid } from './bid'

@Injectable()
export class DataService {

  private url = "/bid";

  constructor(private http: HttpClient) { }

  placeBid(lotId : number, bid : Bid) {
    return this.http.post(this.url + '/' + lotId, bid);
  }
}
