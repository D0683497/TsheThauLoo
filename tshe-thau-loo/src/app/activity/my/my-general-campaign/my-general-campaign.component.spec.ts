import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { MyGeneralCampaignComponent } from './my-general-campaign.component';

describe('MyGeneralCampaignComponent', () => {
  let component: MyGeneralCampaignComponent;
  let fixture: ComponentFixture<MyGeneralCampaignComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ MyGeneralCampaignComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(MyGeneralCampaignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
