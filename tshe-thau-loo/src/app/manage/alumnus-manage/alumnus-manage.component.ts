import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { AlumnusService } from '../../services/manage/alumnus.service';
import { IAlumnus } from '../../models/manage/alumnus.models';

@Component({
  selector: 'app-alumnus-manage',
  templateUrl: './alumnus-manage.component.html',
  styleUrls: ['./alumnus-manage.component.scss'],
})
export class AlumnusManageComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  alumnus: IAlumnus;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private alumnusService: AlumnusService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.alumnusService.getAlumnus(this.userId).subscribe(
      (res: IAlumnus) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IAlumnus): Promise<void> {
    this.alumnus = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  generatePhoto(userId: string): string {
    return createAvatar(style, {
      seed: userId,
      dataUri: true
    });
  }

  toggleNationalId = (): boolean => this.showNationalId = !this.showNationalId;

}
