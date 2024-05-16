import { Component, OnInit } from "@angular/core";
import { DataService } from "../../services/data.service";
import { LotInfo } from "../../model/lotInfo";
import { CategoryInfo } from "../../model/categoryInfo";
import { FormControl, FormGroup } from "@angular/forms";
import { DeliveryInfo } from "../../model/deliveryInfo";
import { MatCalendarCellClassFunction, MatMonthView } from "@angular/material/datepicker";
import { PremiumInfo } from "../../model/premiumInfo";

@Component({
  selector: 'lot-create',
  templateUrl: './lot-create.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-create.component.css'
  ],
  providers: [DataService]
})

export class LotCreateComponent implements OnInit {

  auctionTypes: Map<number, string> = new Map<number, string>();
  conditions: Map<number, string> = new Map<number, string>();

  lotInfo: LotInfo = new LotInfo(3, "", 10);

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

  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  constructor(private dataService: DataService) { }

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
        this.premiumPayment = Math.floor(dif * cat.payment) / 100;
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
      let deliveryInfo = new DeliveryInfo(this.selectedDelivery, del!, this.payment);
      this.lotInfo.deliveryInfos.push(deliveryInfo);
      this.selectableDeliveries.delete(this.selectedDelivery);
      this.selectedCategory = -1;
    }
  }

}
