import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';

@Component({
  templateUrl: './lot-list.component.html',
  providers: [DataService]
})
export class LotListComponent implements OnInit {

  categories: Map<number, string> = new Map();

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.dataService.getCategories().subscribe((data: any) => this.categories = data as typeof this.categories);
  }
}
