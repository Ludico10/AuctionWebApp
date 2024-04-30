import { Component } from "@angular/core";

@Component({
  selector: 'header-section',
  templateUrl: './header.component.html',
  styleUrls: [
    '../css/bootstrap.css',
    '../css/responsive.css',
    '../css/style.css'
  ]
})

export class HeaderComponent {

  navPanel: boolean = false;

  onPanelClick() {
    this.navPanel = !this.navPanel;
  }
}
