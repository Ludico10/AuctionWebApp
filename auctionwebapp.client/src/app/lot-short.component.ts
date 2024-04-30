import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { LotShort } from "./lot-short";
import { DataService } from "./data.service";

@Component({
  selector: "lot-short",
  templateUrl: "./lot-short.component.html",
  styleUrls: [
    '../css/bootstrap.css',
    '../css/responsive.css',
    '../css/style.css',
    './lot-short.component.css'
  ],
  providers: [DataService]
})

export class LotShortComponent implements OnInit {
  @Input() info!: LotShort;
  @Input() auctionType?: string;
  @Output() infoChange = new EventEmitter<LotShort>();
  @Output() auctionTypeChange = new EventEmitter<string>();

  imageToShow: any;
  isImageLoading: boolean = true;

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    this.getImageFromService();
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.imageToShow = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }

  getImageFromService() {
    this.isImageLoading = true;
    this.dataService.getImage(this.info.name).subscribe(data => {
      this.createImageFromBlob(data);
      this.isImageLoading = false;
    }, error => {
      console.log(error);
    });
  }
}
