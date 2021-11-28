import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { RecruitmentCampaignOpeningCreateComponent } from './recruitment-campaign-opening-create.component';

describe('RecruitmentCampaignOpeningCreateComponent', () => {
  let component: RecruitmentCampaignOpeningCreateComponent;
  let fixture: ComponentFixture<RecruitmentCampaignOpeningCreateComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RecruitmentCampaignOpeningCreateComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitmentCampaignOpeningCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
