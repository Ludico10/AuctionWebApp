import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { LotShort } from "../../model/lot-short";
import { DataService } from "../../services/data.service";

@Component({
  selector: "lot-short",
  templateUrl: "./lot-short.component.html",
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-short.component.css'
  ],
  providers: [DataService]
})

export class LotShortComponent implements OnInit {
  @Input() info!: LotShort;
  @Output() infoChange = new EventEmitter<LotShort>();

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
    this.dataService.getImage(this.info.lotId + "-0", "actual").subscribe({
      next: data => {
        this.createImageFromBlob(data);
        this.isImageLoading = false;
      },
      error: (error: any) => { console.log(error) }
    });
  }
}
