import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Pagination } from '../../models/pagination/pagination';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { ExaminerService } from '../../services/manage/examiner.service';
import { IExaminer } from '../../models/manage/examiner.models';

@Component({
  selector: 'app-examiner-manage-list',
  templateUrl: './examiner-manage-list.component.html',
  styleUrls: ['./examiner-manage-list.component.scss'],
})
export class ExaminerManageListComponent implements OnInit {

  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  examiners: IExaminer[];

  constructor(
    private examinerService: ExaminerService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<any> {
    this.examinerService.getExaminers(this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<IExaminer[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<IExaminer[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.examiners = res.body as IExaminer[];
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
