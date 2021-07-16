import { Component, OnInit } from '@angular/core';
import { IAdministrator } from '../../models/manage/administrator.models';
import { BehaviorSubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { AdministratorService } from '../../services/manage/administrator.service';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-administrator-manage',
  templateUrl: './administrator-manage.component.html',
  styleUrls: ['./administrator-manage.component.scss'],
})
export class AdministratorManageComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  administrator: IAdministrator;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private administratorService: AdministratorService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.administratorService.getAdministrator(this.userId).subscribe(
      (res: IAdministrator) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IAdministrator): Promise<void> {
    this.administrator = res;
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
