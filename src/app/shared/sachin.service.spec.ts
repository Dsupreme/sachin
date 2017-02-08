/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SachinService } from './sachin.service';

describe('SachinService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SachinService]
    });
  });

  it('should ...', inject([SachinService], (service: SachinService) => {
    expect(service).toBeTruthy();
  }));
});
