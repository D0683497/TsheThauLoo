import { TestBed } from '@angular/core/testing';

import { GeneralCampaignService } from './general-campaign.service';

describe('GeneralCampaignService', () => {
  let service: GeneralCampaignService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GeneralCampaignService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
