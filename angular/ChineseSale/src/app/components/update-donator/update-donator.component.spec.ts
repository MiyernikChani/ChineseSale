import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDonatorComponent } from './update-donator.component';

describe('UpdateDonatorComponent', () => {
  let component: UpdateDonatorComponent;
  let fixture: ComponentFixture<UpdateDonatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateDonatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDonatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
