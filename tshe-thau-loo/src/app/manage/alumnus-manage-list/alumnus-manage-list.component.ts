import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Pagination } from '../../models/pagination/pagination';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { IAlumnus } from '../../models/manage/alumnus.models';
import { AlumnusService } from '../../services/manage/alumnus.service';

@Component({
  selector: 'app-alumnus-manage-list',
  templateUrl: './alumnus-manage-list.component.html',
  styleUrls: ['./alumnus-manage-list.component.scss'],
})
export class AlumnusManageListComponent implements OnInit {

  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  alumni: IAlumnus[];

  constructor(
    private alumnusService: AlumnusService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<any> {
    this.alumnusService.getAlumni(this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<IAlumnus[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<IAlumnus[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.alumni = res.body as IAlumnus[];
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async next(): Promise<void> {
    this.pagination.pageIndex = this.pagination.pageIndex + this.pagination.pageSize;
    await this.getData();
  }

  async previous(): Promise<void> {
    this.pagination.pageIndex = this.pagination.pageIndex - this.pagination.pageSize;
    await this.getData();
  }

  generatePhoto(userId: string): string {
    return createAvatar(style, {
      seed: userId,
      dataUri: true
    });
  }

}
