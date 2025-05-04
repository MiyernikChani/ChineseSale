import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ServiceService } from '../../services/gifts/gifts.service';
import { DonatorsService } from '../../services/donators/donators.service';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Gift } from '../../models/gift.model';
import { Donator } from '../../models/donator.model';
import { Category } from '../../models/category.model';
import { NgIf } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';
import { UserService } from '../../services/user/user.service';

@Component({
  selector: 'app-add-donator',
  standalone: true,
  imports: [FloatLabel, ButtonModule, DialogModule, ToastModule, CommonModule, ReactiveFormsModule, NgIf],
  templateUrl: './add-donator.component.html',
  styleUrl: './add-donator.component.css',
  providers: [MessageService, ServiceService, DonatorsService, UserService]

})
export class AddDonatorComponent {
  constructor(private userService: UserService, private messageService: MessageService) {
    this.frmDonator = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      phone: new FormControl('', [Validators.required, Validators.minLength(9), Validators.maxLength(10), Validators.pattern(/^\d+$/)]),
      mail: new FormControl('', [Validators.required, Validators.email]),
      address: new FormControl('', [Validators.required]),
    })
  }
  visible: boolean = true;
  frmDonator: FormGroup;

  defaultPrice: number = 10;
  selectedFile: File | null = null;
  image: string | ArrayBuffer | null = null;

  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);

  donatorService: DonatorsService = inject(DonatorsService);
  giftService: ServiceService = inject(ServiceService);

  giftList: Gift[] = [];
  donatorList: Donator[] = [];
  categoryList: Category[] = [];

  ngOnInit() {
    console.log("oninit");
  }

  async addDonator(event: Event) {
    event.preventDefault();
    var donator = new Donator();
    donator.id = 0;
    donator.name = this.frmDonator.controls['name'].value;
    donator.phone = this.frmDonator.controls['phone'].value;
    donator.mail = this.frmDonator.controls['mail'].value;
    donator.address = this.frmDonator.controls['address'].value;
    donator.gifts = [];

    this.donatorService.addDonator(donator).subscribe({
      next: async response => {
        console.log('סטטוס:', response.status);
        console.log(response.message);
        this.showSuccessToast();
      },
      error: HttpErrorResponse => {
        console.log(HttpErrorResponse.status);
        if (HttpErrorResponse.status === 400) {
          this.visible = false;
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'כבר קיימת מתנה בשם זה!' });
          this.close();
        } 
        else if(HttpErrorResponse.status === 200) {
          this.showSuccessToast();
        }
        else {
          console.log("error");
          this.showErrorToast();
        }
      }
    });
  }
  async showErrorToast() {
    this.visible = false;
    this.messageService.add({ severity: 'error', summary: 'Error', detail: 'תורם לא נוסף' });
    this.close();
  }
  async showSuccessToast() {
    this.visible = false;
    this.messageService.add({ severity: 'success', summary: 'add', detail: 'תורם נוסף בהצלחה!' });
    this.close();
  }

  close() {
    setTimeout(() => {
      this.messageService.clear();
      this.route();
    }, 2000);
  }

  route() {
    setTimeout(() => {
      this.router.navigate(['donator']);
    }, 2000);
  }

  shut() {
    this.router.navigate(['donator']);
  }
}
