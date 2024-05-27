import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { DataService } from "../../services/data.service";
import { UserShortInfo } from "../../model/userShortInfo";

@Component({
  selector: 'header-section',
  templateUrl: './header.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css',
    './header.component.css'
  ],
  providers: [DataService]
})

export class HeaderComponent implements OnInit {

  navPanel: boolean = false;
  showInfo: boolean = false;
  roleName: string = "";
  userShort: UserShortInfo | null = null;
  time: any;
  userImg: any;
  imgFound = false;

  constructor(private dataService: DataService, private jwtHelper: JwtHelperService, private router: Router) { }

  ngOnInit(): void {
    let uid = localStorage.getItem("uid");
    let name = localStorage.getItem("uname");
    let roleId = localStorage.getItem("urole");
    let rating = localStorage.getItem("rating");
    if (uid && name && roleId && rating) {
      this.userShort = new UserShortInfo(Number.parseInt(uid), name, Number.parseInt(roleId), Number.parseInt(rating));
      this.dataService.getRoleName(this.userShort.roleId).subscribe((data: any) => this.roleName = data as string);
      this.dataService.getImage(this.userShort.id.toString(), "users").subscribe({
        next: (image: any) => {
          this.createImageFromBlob(image);
          this.imgFound = true;
        },
        error: () => { }
      });
    }
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.userImg = reader.result;
    }, false);
    if (image) {
      reader.readAsDataURL(image);
    }
  }

  onPanelClick() {
    this.navPanel = !this.navPanel;
    this.showInfo = false;
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

  onUserEnter() {
    clearTimeout(this.time);
    this.showInfo = true;
  }

  onUserLeave() {
    this.time = setTimeout(() => {
      this.showInfo = false;
    }, 2000);
  }

  logOut = () => {
    localStorage.clear();
    this.showInfo = false;
    this.userShort = null;
    this.imgFound = false;
    this.router.navigate(["/"]);
  }
}
