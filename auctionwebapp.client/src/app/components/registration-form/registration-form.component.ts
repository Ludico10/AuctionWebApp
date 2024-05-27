import { Component, OnInit } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

import { DataService } from "../../services/data.service";
import { RegistrationInfo } from "../../model/registrationInfo";
import { TokenApiModel } from "../../model/tokenApiModel";
import { UserShortInfo } from "../../model/userShortInfo";

@Component({
  selector: 'app-registration',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css'],
  providers: [DataService]
})

export class RegistrationComponent implements OnInit {

  info: RegistrationInfo = new RegistrationInfo;
  passwordCheck: string = "";
  invalidParams: boolean = true;

  constructor(private dataService: DataService, private router: Router) { }

  ngOnInit() { }

  save() {
    if (this.info.name && this.info.email && this.info.passwordHash && this.info.passwordHash == this.passwordCheck) {
      this.dataService.registrate(this.info).subscribe({
        next: (response: UserShortInfo) => {
          if (response.tokens) {
            const token = response.tokens.accessToken;
            const refreshToken = response.tokens.refreshToken;
            localStorage.setItem("jwt", token);
            localStorage.setItem("refreshToken", refreshToken);
            localStorage.setItem("uid", response.id.toString());
            localStorage.setItem("urole", response.roleId.toString());
            localStorage.setItem("uname", response.name);
            localStorage.setItem("rating", response.rating.toString());
            this.invalidParams = false;
            this.router.navigate(["/"]);
          }
        },
        error: (err: HttpErrorResponse) => this.invalidParams = true
      });
    }
  }
}
