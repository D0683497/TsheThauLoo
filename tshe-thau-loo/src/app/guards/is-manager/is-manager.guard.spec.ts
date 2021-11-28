import { TestBed } from '@angular/core/testing';

import { IsManagerGuard } from './is-manager.guard';

describe('IsManagerGuard', () => {
  let guard: IsManagerGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(IsManagerGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
