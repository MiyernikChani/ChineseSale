import { Component } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { ServiceService } from '../../services/gifts/gifts.service';
import { inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { DonatorsService } from '../../services/donators/donators.service';
import { Donator } from '../../models/donator.model';
import { Category } from '../../models/category.model';
import { FileUploadModule } from 'primeng/fileupload';
import { ButtonModule } from 'primeng/button';
import { HttpClient } from '@angular/common/http';
import { DialogModule } from 'primeng/dialog';
import { HttpClientModule } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-update-donator',
  standalone: true,
  imports: [ConfirmDialog, ToastModule, HttpClientModule, DialogModule, ButtonModule, NgIf, ReactiveFormsModule, CommonModule, FileUploadModule],
  templateUrl: './update-donator.component.html',
  styleUrl: './update-donator.component.css',
  providers: [MessageService, ServiceService, HttpClient, DonatorsService]
})
export class UpdateDonatorComponent {
  constructor(private cdr: ChangeDetectorRef,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService
  ) {
    this.frmDonator = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
      phone: new FormControl('', [Validators.required, Validators.minLength(9), Validators.maxLength(10), Validators.pattern(/^\d+$/)]),
      mail: new FormControl('', [Validators.required, Validators.email]),
      address: new FormControl('', [Validators.required]),
    })
  }

  frmDonator: FormGroup;

  donatorId: number = 0;
  donatorList: Donator[] = [];
  donatorById: any | null = null;

  visible: boolean = true;

  donatorService: DonatorsService = inject(DonatorsService);
  giftService: ServiceService = inject(ServiceService);

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      if (params['id'] && params['id'] > 0) {
        this.donatorId = params['id'];
        this.data();
      }
    });
  }

  data() {
    this.getDonators();
  }

  getDonators() {
    this.giftService.getAllGifts().subscribe((data: any) => {
      this.donatorList = data;
      this.selectDonatorList();
    });
  }

  selectDonatorList() {
    this.donatorService.getAllDonators().subscribe((data: Donator[]) => {
      this.donatorList = data;
      this.giftService.getAllCategories().subscribe((data: any) => {
        console.log(this.donatorList);
      });
      try {
        this.donatorService.getAllDonators().subscribe((data: any) => {
          this.donatorList = data;
          this.findDonator();
        });
      } catch {
        console.log("not athourizied");
        this.messageService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
      }

      this.cdr.detectChanges(); // הוסף כאן כדי לודא ש-Angular יודע על השינויים
    });
  }

  findDonator() {
    this.donatorList.forEach(element => {
      if (element.id == this.donatorId)
        this.donatorById = element;
    });

    if (this.donatorById) {
      this.frmDonator.controls['name'].setValue(this.donatorById.name);
      this.frmDonator.controls['phone'].setValue(this.donatorById.phone);
      this.frmDonator.controls['mail'].setValue(this.donatorById.mail);
      this.frmDonator.controls['address'].setValue(this.donatorById.address);
    }
  }

  async updateDonator(event: Event) {
    event.preventDefault();
    const donator = new Donator();
    donator.id = this.donatorId;
    donator.name = this.frmDonator.controls['name'].value;
    donator.phone = this.frmDonator.controls['phone'].value;
    donator.mail = this.frmDonator.controls['mail'].value;
    donator.address = this.frmDonator.controls['address'].value;
    donator.gifts = this.donatorById.gifts;

    this.donatorService.updateDonator(donator).subscribe({
      next: async response => {
        console.log('סטטוס:', response.status);
        console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
        await this.showSuccessToast();
      },
      error: error => {
        if (error.status === 400) {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'כבר קיים תורם בשם זה' });
          setTimeout(() => {
            this.messageService.clear(); // מנקה את כל הודעות הטוסט
          }, 3000);
        }
        else {
          console.error('שגיאה בעדכון תורם:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'תורם לא עודכן' });
          setTimeout(() => {
            this.messageService.clear(); // מנקה את כל הודעות הטוסט
          }, 3000);
        }
      }
    });
  }
  async showSuccessToast() {
    console.log("visible");
    this.visible = false;
    this.messageService.add({ severity: 'success', summary: 'update', detail: 'תורם עודכן בהצלחה!' });
    this.close();
  }

  close() {
    setTimeout(() => {
      this.messageService.clear();
      this.route();
    }, 2000);
  }

  route() {
    this.router.navigate(['donator']);
  }
}
