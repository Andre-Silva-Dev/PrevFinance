import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DebtShell } from './debt-shell';

describe('DebtShell', () => {
  let component: DebtShell;
  let fixture: ComponentFixture<DebtShell>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DebtShell]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DebtShell);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
