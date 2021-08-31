import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public title: string = 'Cars Info';
  public footerText: string = `${new Date().getUTCFullYear()} Cars Info`;
}
