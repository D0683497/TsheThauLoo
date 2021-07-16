import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Pagination } from '../../models/pagination/pagination';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { StudentService } from '../../services/manage/student.service';
import { IStudent } from '../../models/manage/student.models';

@Component({
  selector: 'app-student-manage-list',
  templateUrl: './student-manage-list.component.html',
  styleUrls: ['./student-manage-list.component.scss'],
})
export class StudentManageListComponent implements OnInit {

  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  students: IStudent[];

  constructor(
    private studentService: StudentService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<any> {
    this.studentService.getStudents(this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<IStudent[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<IStudent[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.students = res.body as IStudent[];
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
