import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiciosHotel } from './servicios-hotel';

describe('ServiciosHotel', () => {
  let component: ServiciosHotel;
  let fixture: ComponentFixture<ServiciosHotel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiciosHotel]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServiciosHotel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
