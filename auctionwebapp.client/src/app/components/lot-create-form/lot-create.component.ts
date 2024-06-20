import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { DataService } from "../../services/data.service";
import { LotInfo } from "../../model/lotInfo";
import { CategoryInfo } from "../../model/categoryInfo";
import { FormControl, FormGroup } from "@angular/forms";
import { DeliveryInfo } from "../../model/deliveryInfo";
import { MatCalendarCellClassFunction, MatMonthView } from "@angular/material/datepicker";
import { PremiumInfo } from "../../model/premiumInfo";
import { Router } from "@angular/router";
import { TimeService } from "../../services/time.service";
import { HttpEventType, HttpResponse } from "@angular/common/http";
import { FileUploadService } from "../../services/file-upload.service";
import { Observable } from "rxjs";

@Component({
  selector: 'lot-create',
  templateUrl: './lot-create.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-create.component.css',
    '../lot-detail/lot-detail.component.css'
  ],
  providers: [DataService, TimeService, FileUploadService]
})

export class LotCreateComponent implements OnInit {

  auctionTypes: Map<number, string> = new Map<number, string>();
  conditions: Map<number, string> = new Map<number, string>();

  data: any;
  lotInfo: LotInfo = new LotInfo(0, "", 0);

  selectedCategory: number = -1;
  categories: Array<PremiumInfo> = new Array<PremiumInfo>();
  selectableCategories: Array<PremiumInfo> = new Array<PremiumInfo>();
  payments: Array<number> = new Array<number>();
  premiumPayment: number = 0;

  selectedDelivery: number = -1;
  selectableDeliveries: Map<number, string> = new Map<number, string>();
  payment: number = 0;

  initCost: number = 0;
  costStep: number = 0;

  paramKey: string = "";
  paramValue: string = "";

  year: number = 2024;
  month: number = 1;
  freeDates: Array<number> = new Array<number>();

  finishTimeString: string = "";
  finishDate: Date = new Date();

  startPremDate: Date = new Date();
  finishPremDate: Date = new Date();

  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  images: Map<number, any> = new Map<number, any>();
  imageInfos?: Observable<any>;

  selectedFiles?: FileList;
  currentFile?: File;
  progress = 0;
  message = '';
  previews: Array<string> = new Array<string>();
  mainId = 0;

  @ViewChild("file") file: ElementRef | undefined;
  @ViewChild("filee") filee: ElementRef | undefined;

  constructor(private dataService: DataService, private timeService: TimeService, private uploadService: FileUploadService, private router: Router)
  {
    this.data = this.router.getCurrentNavigation()?.extras.state?.['lotInfo'];
  }

  ngOnInit() {
    this.imageInfos = this.uploadService.getFiles();

      this.dataService.getCategoriesPremium().subscribe((data: Array<PremiumInfo>) => {
        this.selectableCategories = data;
        this.categories = data;
      });

      this.dataService.getConditions().subscribe((data: any) => {
        this.conditions = data as typeof this.conditions;
      });

      this.dataService.getAuctionTypes().subscribe((data: any) => {
        this.auctionTypes = data as typeof this.auctionTypes;
      });

      this.dataService.getDeliveries().subscribe((data: any) => {
        let deliveries = data as typeof this.selectableDeliveries;
        for (let i = 0; i < Object.keys(deliveries).length; i++) {
          let key = parseInt(Object.keys(deliveries)[i]);
          let value = Object.values(deliveries)[i];
          this.selectableDeliveries.set(key, value);
        }
      });

    let uidStr = localStorage.getItem("uid");
    let unameStr = localStorage.getItem("uname");
    let ratingStr = localStorage.getItem("rating");
    if (uidStr && unameStr && ratingStr) {
      if (this.data) {
        this.lotInfo = this.data as typeof this.lotInfo;

        this.lotInfo.categoryInfos.forEach(cat => {
          if (this.selectableCategories.some(c => c.categoryId == cat.categoryId)) {
            let index = this.selectableCategories.findIndex(c => c.categoryId == cat.categoryId);
            this.selectableCategories.splice(index, 1);
          }
        });

        this.lotInfo.deliveryInfos.forEach(del => {
          if (this.selectableDeliveries.has(del.countryId)) {
            this.selectableDeliveries.delete(del.countryId);
          }
        });

        this.initCost = this.lotInfo.initialCost / 100;
        this.costStep = this.lotInfo.costStep / 100;

        let timeArr = this.timeService.getTimeFromMilli(this.lotInfo.finishTime);
        this.finishTimeString = this.timeService.timeToString(timeArr[0], timeArr[1]);
        this.finishDate = this.timeService.getDateFromMilli(this.lotInfo.finishTime);

      } else {
        this.lotInfo = new LotInfo(Number.parseInt(uidStr), unameStr, Number.parseInt(ratingStr));
      }
    } else {
      this.router.navigateByUrl("/");
    }
  }

  ngOnDestroy() {
    this.router.getCurrentNavigation()?.extras
  }

  dateClass: MatCalendarCellClassFunction<Date> = (cellDate, view) => {
    if (view === 'month') {
      let year = cellDate.getFullYear();
      let month = cellDate.getMonth();
      if (year !== this.year || month !== this.month) {
        this.dataService.getFreeDays(this.selectedCategory, year, month).subscribe((data: Array<number>) => {
          this.freeDates = data;
          const date = cellDate.getDate();
          let inside = this.freeDates.includes(date);
          this.year = year;
          this.month = month;
          return inside ? 'example-custom-date-class' : '';
        });
      }else {
        const date = cellDate.getDate();
        let inside = this.freeDates.includes(date);
        return inside ? 'example-custom-date-class' : '';
      }
    }
    return 'example-custom-date-class';
  };

  dateChange() {
    if (this.selectedCategory >= 0) {
      let startDate = this.range.get('start')!.value!;
      let endDate = this.range.get('end')!.value!;
      let dif = (endDate.getTime() - startDate.getTime()) / (1000 * 3600 * 24) + 1;
      let cat = this.selectableCategories.find(c => c.categoryId == this.selectedCategory);
      if (cat) {
        this.premiumPayment = dif * cat.payment / 100;
      }
    }
  }

  addParam() {
    if (this.paramKey) {
      let value = "";
      if (this.paramValue) value = this.paramValue;
      this.lotInfo.parameters.set(this.paramKey, value);
    }
  }

  deleteParam(key: string) {
    if (this.lotInfo.parameters.has(key)) {
      this.lotInfo.parameters.delete(key);
    }
  }

  deleteCategory(row: number) {
    if (this.lotInfo.categoryInfos.length >= row) {
      let categoryInfo = this.lotInfo.categoryInfos[row];
      let cat = this.categories.find(cat => cat.categoryId == categoryInfo.categoryId);
      if (cat) {
        this.selectableCategories.push(cat);
        this.lotInfo.categoryInfos.splice(row, 1);
        this.payments.splice(row, 1);
      }
    }
  }

  addCategory() {
    let cat = this.selectableCategories.find(cat => cat.categoryId == this.selectedCategory)
    if (cat) {
      let categoryInfo = new CategoryInfo(this.selectedCategory, cat.categoryName, this.range.get('start')!.value!, this.range.get('end')!.value!);
      this.lotInfo.categoryInfos.push(categoryInfo);
      this.payments.push(this.premiumPayment);
      let index = this.selectableCategories.lastIndexOf(cat);
      this.selectableCategories.splice(index, 1);
      this.selectedCategory = -1;
      this.premiumPayment = 0;
    }
  }

  deleteDelivery(row: number) {
    if (this.lotInfo.deliveryInfos.length >= row) {
      let deliveryInfo = this.lotInfo.deliveryInfos[row];
      this.selectableDeliveries.set(deliveryInfo.countryId, deliveryInfo.countryName);
      this.lotInfo.deliveryInfos.splice(row, 1);
    }
  }

  addDelivery() {
    if (this.selectableDeliveries.has(this.selectedDelivery)) {
      let del = this.selectableDeliveries.get(this.selectedDelivery);
      let deliveryInfo = new DeliveryInfo(this.selectedDelivery, del!, Math.floor(this.payment * 100));
      this.lotInfo.deliveryInfos.push(deliveryInfo);
      this.selectableDeliveries.delete(this.selectedDelivery);
      this.selectedCategory = -1;
    }
  }

  save() {
    this.lotInfo.initialCost = Math.floor(this.initCost * 100);
    this.lotInfo.costStep = Math.floor(this.costStep * 100);

    let dateMilli = this.finishDate.getTime();
    let timeMilli = this.timeService.parseTime(this.finishTimeString);
    if (dateMilli && timeMilli) {
      this.lotInfo.finishTime = dateMilli + timeMilli;
    }

    this.dataService.placeLot(this.lotInfo).subscribe((data: LotInfo) => {
      if (data.id) this.upload(data.id);
      this.router.navigateByUrl("/");
    });
  }

  selectFile(event: any): void {
    this.progress = 0;
    let newFiles = event.target.files;
    this.selectedFiles = newFiles;
    if (newFiles) {
      for (let j = 0; j < newFiles.length && j < 5 - this.selectedFiles!.length; j++) {
          const file: File | null = newFiles.item(j);
          if (file) {
            this.currentFile = file;
            const reader = new FileReader();
            reader.onload = (e: any) => {
              console.log(e.target.result);
              this.previews.push(e.target.result);
            };
            reader.readAsDataURL(this.currentFile);
          }
      }
      this.selectedFiles += newFiles;
    }
  }

  triggerUpload(event: any) {
    this.file!.nativeElement.click(); 
  }

  trigggerUpload(event: any) {
    this.filee!.nativeElement.click();
  }

  selectImage(i: number) {
    this.mainId = i;
  }

  onMouseEnter(hoverName: HTMLElement) {
    hoverName.style.border = "1px";
    hoverName.style.borderStyle = "solid";
  }

  onMouseOut(hoverName: HTMLElement) {
    hoverName.style.border = "0px";
  }

  onEnter(hoverName: HTMLElement, hoverButton: HTMLElement) {
    this.onMouseEnter(hoverName);
    hoverButton.style.backgroundColor = "#252525";
  }

  onOut(hoverName: HTMLElement, hoverButton: HTMLElement) {
    this.onMouseOut(hoverName);
    hoverButton.style.backgroundColor = "#9d2222";
  }

  deleteImage(i: number) {
    if (i <= this.mainId) this.mainId--;
    this.previews.splice(i, 1);
  }

  upload(lotId: number): void {
    this.progress = 0;

    if (this.selectedFiles) {
      for (let i = 0; i < this.selectedFiles.length && i < 5; i++) {
        const file: File | null = this.selectedFiles[i];
        if (file) {
          this.currentFile = file;
          let id = 0;
          if (i < this.mainId) id = i - 1;
          if (i > this.mainId) id = i;
          this.uploadService.upload(this.currentFile, lotId, id).subscribe({
            next: (event: any) => {
              if (event.type === HttpEventType.UploadProgress) {
                this.progress = Math.round((100 * event.loaded) / event.total);
              } else if (event instanceof HttpResponse) {
                this.message = event.body.message;
                this.imageInfos = this.uploadService.getFiles();
              }
            },
            error: (err: any) => {
              console.log(err);
              this.progress = 0;

              if (err.error && err.error.message) {
                this.message = err.error.message;
              } else {
                this.message = 'Could not upload the image!';
              }

              this.currentFile = undefined;
            },
          });
        }
      }

      this.selectedFiles = undefined;
    }
  }
}
