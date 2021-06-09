import { TestBed } from '@angular/core/testing';

import { AlumnusService } from './alumnus.service';

describe('AlumnusService', () => {
  let service: AlumnusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AlumnusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
