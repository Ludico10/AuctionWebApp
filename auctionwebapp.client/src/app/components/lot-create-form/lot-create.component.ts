import { Component, OnInit } from "@angular/core";
import { DataService } from "../../services/data.service";
import { LotInfo } from "../../model/lotInfo";
import { CategoryInfo } from "../../model/categoryInfo";
import { FormControl, FormGroup } from "@angular/forms";
import { DeliveryInfo } from "../../model/deliveryInfo";
import { MatCalendarCellClassFunction, MatMonthView } from "@angular/material/datepicker";
import { PremiumInfo } from "../../model/premiumInfo";
import { Router } from "@angular/router";
import { TimeService } from "../../services/time.service";

@Component({
  selector: 'lot-create',
  templateUrl: './lot-create.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-create.component.css'
  ],
  providers: [DataService, TimeService]
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

  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  constructor(private dataService: DataService, private timeService: TimeService, private router: Router)
  {
    this.data = this.router.getCurrentNavigation()?.extras.state?.['lotInfo'];
  }

  ngOnInit() {
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
    this.router.getCurrentNavigation();
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
    return '';
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
      let value = undefined;
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

    this.dataService.placeLot(this.lotInfo).subscribe(() => {
      this.router.navigateByUrl("/");
    });
  }
}
