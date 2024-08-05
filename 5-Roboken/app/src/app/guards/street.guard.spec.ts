import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { StreetGuard } from './street.guard';

describe('streetGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => StreetGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
