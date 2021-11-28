import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { RecruitmentCampaignOpeningEditComponent } from './recruitment-campaign-opening-edit.component';

describe('RecruitmentCampaignOpeningEditComponent', () => {
  let component: RecruitmentCampaignOpeningEditComponent;
  let fixture: ComponentFixture<RecruitmentCampaignOpeningEditComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RecruitmentCampaignOpeningEditComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(RecruitmentCampaignOpeningEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
