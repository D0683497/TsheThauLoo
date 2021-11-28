import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RoleType } from '../../../enums/role-type.enum';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { OpeningService } from '../../../services/opening/opening.service';
import { IOpening } from '../../../models/opening/opening.model';

@Component({
  selector: 'app-recruitment-campaign-opening-list',
  templateUrl: './recruitment-campaign-opening-list.component.html',
  styleUrls: ['./recruitment-campaign-opening-list.component.scss'],
})
export class RecruitmentCampaignOpeningListComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  recruitmentId = this.route.snapshot.paramMap.get('recruitmentId');
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  openings: IOpening[];
  type = RoleType;

  constructor(
    private route: ActivatedRoute,
    private openingService: OpeningService,
    public authService: AuthService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.openingService.getOpenings(this.campaignId, this.recruitmentId).subscribe(
      (res: IOpening[]) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IOpening[]): Promise<void> {
    this.openings = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

}
