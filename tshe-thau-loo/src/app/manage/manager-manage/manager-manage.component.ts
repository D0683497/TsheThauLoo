import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { IManager } from '../../models/manage/manager.models';
import { ManagerService } from '../../services/manage/manager.service';

@Component({
  selector: 'app-manager-manage',
  templateUrl: './manager-manage.component.html',
  styleUrls: ['./manager-manage.component.scss'],
})
export class ManagerManageComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  manager: IManager;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private managerService: ManagerService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.managerService.getManager(this.userId).subscribe(
      (res: IManager) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IManager): Promise<void> {
    this.manager = res;
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
