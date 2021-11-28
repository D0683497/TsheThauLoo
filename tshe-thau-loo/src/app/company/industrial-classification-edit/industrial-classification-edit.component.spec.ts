import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { IndustrialClassificationEditComponent } from './industrial-classification-edit.component';

describe('IndustrialClassificationEditComponent', () => {
  let component: IndustrialClassificationEditComponent;
  let fixture: ComponentFixture<IndustrialClassificationEditComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustrialClassificationEditComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(IndustrialClassificationEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
