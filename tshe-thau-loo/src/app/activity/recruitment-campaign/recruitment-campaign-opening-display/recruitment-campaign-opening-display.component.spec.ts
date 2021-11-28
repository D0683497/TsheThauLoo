import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { RecruitmentCampaignOpeningDisplayComponent } from './recruitment-campaign-opening-display.component';

describe('RecruitmentCampaignOpeningDisplayComponent', () => {
  let component: RecruitmentCampaignOpeningDisplayComponent;
  let fixture: ComponentFixture<RecruitmentCampaignOpeningDisplayComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RecruitmentCampaignOpeningDisplayComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitmentCampaignOpeningDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
