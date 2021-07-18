import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { GeneralCampaignParticipateComponent } from './general-campaign-participate.component';

describe('GeneralCampaignParticipateComponent', () => {
  let component: GeneralCampaignParticipateComponent;
  let fixture: ComponentFixture<GeneralCampaignParticipateComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralCampaignParticipateComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(GeneralCampaignParticipateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
