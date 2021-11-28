import { TestBed } from '@angular/core/testing';

import { RecruitmentCampaignService } from './recruitment-campaign.service';

describe('RecruitmentCampaignService', () => {
  let service: RecruitmentCampaignService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecruitmentCampaignService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
