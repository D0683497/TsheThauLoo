import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { RecruitmentCampaignDisplayComponent } from './recruitment-campaign-display.component';

describe('RecruitmentCampaignDisplayComponent', () => {
  let component: RecruitmentCampaignDisplayComponent;
  let fixture: ComponentFixture<RecruitmentCampaignDisplayComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RecruitmentCampaignDisplayComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitmentCampaignDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
