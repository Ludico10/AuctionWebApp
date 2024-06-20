import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { LotShort } from "../../model/lot-short";
import { DataService } from "../../services/data.service";
import { TimeService } from "../../services/time.service";
import { ModalService } from "../../services/modal.service";
import { Router } from "@angular/router";
import { ComplaintRequest } from "../../model/complaintRequest";

@Component({
  selector: "lot-short",
  templateUrl: "./lot-short.component.html",
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './lot-short.component.css',
    '../registration-form/registration-form.component.css'
  ],
  providers: [DataService, TimeService]
})

export class LotShortComponent implements OnInit {
  @Input() info!: LotShort;
  @Output() infoChange = new EventEmitter<LotShort>();

  @Input() forTrack: boolean = false;
  @Output() forTrackChange = new EventEmitter<boolean>();

  @Input() autoBid?: number;
  @Output() autoBidChange = new EventEmitter<number>();

  @Input() size?: number;
  @Output() sizeChange = new EventEmitter<number>();

  @Input() time?: Date;
  @Output() timeChange = new EventEmitter<Date>();

  @Input() forFinnished: boolean = false;
  @Output() forFinnishedChange = new EventEmitter<boolean>();

  @Input() reasons?: Map<number, string>;
  @Output() reasonsChange = new EventEmitter<Map<number, string>>();

  imageToShow: any;
  isImageLoading: boolean = true;

  timeStr: string = "";

  complaint: ComplaintRequest = new ComplaintRequest();

  constructor(private dataService: DataService, private timeService: TimeService, protected modalService: ModalService, private router: Router) { }

  ngOnInit(): void {
    this.getImageFromService();
    if (this.time) {
      this.timeStr = this.timeService.dateToString(this.time);
    }
    this.complaint.lotId = this.info.lotId;
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

  modalClick(e: Event) {
    this.modalService.open('modal-1');
    e.stopPropagation();
  }

  allClick() {
    this.router.navigate(['/lot'], { queryParams: { "id": this.info.lotId } });
  }

  sendComplaint() {
    this.dataService.placeComlaint(this.complaint).subscribe(() => this.modalService.close());
  }
}
