import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { RecruitmentCampaignOpeningListComponent } from './recruitment-campaign-opening-list.component';

describe('RecruitmentCampaignOpeningListComponent', () => {
  let component: RecruitmentCampaignOpeningListComponent;
  let fixture: ComponentFixture<RecruitmentCampaignOpeningListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RecruitmentCampaignOpeningListComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitmentCampaignOpeningListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
