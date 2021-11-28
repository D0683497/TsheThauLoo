import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { RecruitmentCampaignCreateComponent } from './recruitment-campaign-create.component';

describe('RecruitmentCampaignCreateComponent', () => {
  let component: RecruitmentCampaignCreateComponent;
  let fixture: ComponentFixture<RecruitmentCampaignCreateComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RecruitmentCampaignCreateComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitmentCampaignCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
