import { Component, OnInit } from "@angular/core";
import { LoginInfo } from "../../model/loginInfo";
import { DataService } from "../../services/data.service";
import { NgForm } from "@angular/forms";
import { TokenApiModel } from "../../model/tokenApiModel";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login-form.component.html',
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
          next: (response: TokenApiModel) => {
            const token = response.accessToken;
            const refreshToken = response.refreshToken;
            localStorage.setItem("jwt", token);
            localStorage.setItem("refreshToken", refreshToken);
            this.invalidLogin = false;
            this.router.navigate(["/"]);
          },
          error: (err: HttpErrorResponse) => this.invalidLogin = true
        })
    }
  }
}
