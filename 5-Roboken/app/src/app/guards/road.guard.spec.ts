import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { roadGuard } from './road.guard';

describe('roadGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => roadGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
