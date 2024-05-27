import { Component, OnInit } from "@angular/core";
import { LoginInfo } from "../../model/loginInfo";
import { DataService } from "../../services/data.service";
import { NgForm } from "@angular/forms";
import { TokenApiModel } from "../../model/tokenApiModel";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { UserShortInfo } from "../../model/userShortInfo";

@Component({
  selector: 'app-login',
  templateUrl: './login-form.component.html',
  styleUrls: ['../registration-form/registration-form.component.css'],
  providers: [DataService]
})

export class LoginComponent implements OnInit {
  invalidLogin: boolean = true;
  credentials: LoginInfo = new LoginInfo();

  constructor(private dataService: DataService, private router: Router) { }

  ngOnInit(): void { }

  login = (form: NgForm) => {
    if (form.valid) {
      this.dataService.login(this.credentials)
        .subscribe({
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
              this.invalidLogin = false;
              this.router.navigate(["/"]);
            }
          },
          error: (err: HttpErrorResponse) => this.invalidLogin = true
        })
    }
  }
}
