import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";

@Component({
  selector: "check-all",
  templateUrl: "./check-all.component.html"
})

export class CheckAllComponent implements OnInit {

  allCategories: boolean = true;

  @Input() title: string = "";
  @Input() categories: Map<number, string> = new Map();
  @Input() catChecked: Array<boolean> = new Array();
  @Output() titleChange = new EventEmitter<string>();
  @Output() infoChange = new EventEmitter<Map<number, string>>();
  @Output() auctionTypeChange = new EventEmitter<Array<boolean>>();

  ngOnInit(): void {
  }

  updateAllCategories() {
    let all: boolean = true;
    this.catChecked.forEach(cat => all = all && cat);
    this.allCategories = all;
  }

  setAllCategories(completed: boolean) {
    this.allCategories = completed;
    for (let i = 0; i < Object.keys(this.categories).length; i++) {
      this.catChecked[i] = this.allCategories;
    }
  }
}
