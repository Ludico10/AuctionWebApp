import { Component, OnInit } from "@angular/core";
import { DataService } from "../../services/data.service";
import { LotInfo } from "../../model/lotInfo";
import { CategoryInfo } from "../../model/categoryInfo";
import { FormControl, FormGroup } from "@angular/forms";
import { DeliveryInfo } from "../../model/deliveryInfo";

@Component({
  selector: 'lot-create',
  templateUrl: './lot-create.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
  ],
  providers: [DataService]
})

export class LotCreateComponent implements OnInit {

  auctionTypes: Map<number, string> = new Map<number, string>();
  conditions: Map<number, string> = new Map<number, string>();

  lotInfo: LotInfo = new LotInfo(3, "", 10);

  selectedCategory: number = 0;
  selectableCategories: Map<number, string> = new Map<number, string>();

  selectedDelivery: number = 0;
  selectableDeliveries: Map<number, string> = new Map<number, string>();
  payment: number = 0;

  initCost: number = 0;
  costStep: number = 0;

  paramKey: string = "";
  paramValue: string = "";

  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  constructor(private dataService: DataService) { }

  ngOnInit() {
      this.dataService.getCategories(false).subscribe((data: any) => {
        let categories = data as typeof this.selectableCategories;
        for (let i = 0; i < Object.keys(categories).length; i++) {
          let key = parseInt(Object.keys(categories)[i]);
          let value = Object.values(categories)[i];
          this.selectableCategories.set(key, value);
        }
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
      this.selectableCategories.set(categoryInfo.categoryId, categoryInfo.categoryName);
      this.lotInfo.categoryInfos.splice(row, 1);
    }
  }

  addCategory() {
    if (this.selectableCategories.has(this.selectedCategory)) {
      let cat = this.selectableCategories.get(this.selectedCategory);
      let categoryInfo = new CategoryInfo(this.selectedCategory, cat!, this.range.get('start')!.value!, this.range.get('end')!.value!);
      this.lotInfo.categoryInfos.push(categoryInfo);
      this.selectableCategories.delete(this.selectedCategory);
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
    }
  }

}
