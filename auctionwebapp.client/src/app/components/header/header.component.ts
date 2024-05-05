import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: 'header-section',
  templateUrl: './header.component.html',
  styleUrls: [
    '../../../css/bootstrap.css',
    '../../../css/responsive.css',
    '../../../css/style.css'
  ]
})

export class HeaderComponent implements OnInit {

  navPanel: boolean = false;

  constructor(private jwtHelper: JwtHelperService, private router: Router) { }
    ngOnInit(): void { }

  onPanelClick() {
    this.navPanel = !this.navPanel;
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

  logOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
    this.router.navigate(["/"]);
  }
}
