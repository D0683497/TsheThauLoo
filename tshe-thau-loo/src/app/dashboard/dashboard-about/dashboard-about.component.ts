import { Component, OnInit } from '@angular/core';
import { IAbout } from '../../models/about.model';
import { BehaviorSubject } from 'rxjs';
import { RootService } from '../../services/root/root.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-dashboard-about',
  templateUrl: './dashboard-about.component.html',
  styleUrls: ['./dashboard-about.component.scss'],
})
export class DashboardAboutComponent implements OnInit {

  date = Date.now();
  abouts: IAbout[];
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private rootService: RootService) { }

  ngOnInit(): void {
    this.rootService.about().subscribe(
      (res: IAbout[]) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAbout[]): void {
    this.abouts = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  getFail(err: HttpErrorResponse): void {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

}
