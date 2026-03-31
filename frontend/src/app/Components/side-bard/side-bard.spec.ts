import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBard } from './side-bard';

describe('SideBard', () => {
  let component: SideBard;
  let fixture: ComponentFixture<SideBard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideBard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideBard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
