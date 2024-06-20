import { Component, OnInit } from "@angular/core";
import { DataService } from "../../services/data.service";
import { sectionShort } from "../../model/sectionShort";

@Component({
  selector: "info",
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.css'],
  providers: [DataService]
})

export class InfoComponent implements OnInit {
  pages: Array<sectionShort> = new Array();
  mainPages: Array<sectionShort> = new Array<sectionShort>();
  curPage = 0;
  text: string = "";

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.dataService.getInfoNames().subscribe((data: Array<sectionShort>) => {
      this.pages = data;
      this.pages.forEach(page => {
        if (!page.parentSectionId)
          this.mainPages.push(page);
      });
      if (this.mainPages.length > 0) {
        this.curPage = this.mainPages[0].id;
        this.getPageText();
      }
    });
  }

  getPageText() {
    this.dataService.getCategoryText(this.curPage)
      .subscribe((data: any) => {
        this.text = data as typeof this.text;
        /*for (let i = 0; i < this.text.length; i++) {
          this.text = this.text.replace('+', '<').replace('=', '>');
        }*/
      });
  }

  selectPage(id: number) {
    this.curPage = id;
    this.getPageText();
  }
}
