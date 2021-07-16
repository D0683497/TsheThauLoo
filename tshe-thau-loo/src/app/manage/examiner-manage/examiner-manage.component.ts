import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';
import { IExaminer } from '../../models/manage/examiner.models';
import { ExaminerService } from '../../services/manage/examiner.service';

@Component({
  selector: 'app-examiner-manage',
  templateUrl: './examiner-manage.component.html',
  styleUrls: ['./examiner-manage.component.scss'],
})
export class ExaminerManageComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  examiner: IExaminer;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private examinerService: ExaminerService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.examinerService.getExaminer(this.userId).subscribe(
      (res: IExaminer) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IExaminer): Promise<void> {
    this.examiner = res;
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
