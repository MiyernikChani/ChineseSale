import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftsManagmentComponent } from './gifts-managment.component';

describe('GiftsManagmentComponent', () => {
  let component: GiftsManagmentComponent;
  let fixture: ComponentFixture<GiftsManagmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftsManagmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftsManagmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
