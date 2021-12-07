import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyAnnouncementComponent } from './privacy-announcement.component';

describe('PrivacyAnnouncementComponent', () => {
  let component: PrivacyAnnouncementComponent;
  let fixture: ComponentFixture<PrivacyAnnouncementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivacyAnnouncementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyAnnouncementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
