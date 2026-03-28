import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectionShell } from './projection-shell';

describe('ProjectionShell', () => {
  let component: ProjectionShell;
  let fixture: ComponentFixture<ProjectionShell>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectionShell]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectionShell);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
