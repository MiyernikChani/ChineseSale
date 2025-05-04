import { Component, OnInit } from '@angular/core';
import { Donator } from '../../models/donator.model';
import { DonatorsService } from '../../services/donators/donators.service';
import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { ServiceService } from '../../services/gifts/gifts.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule } from '@angular/forms';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ToastModule } from 'primeng/toast';
import { RouterOutlet } from '@angular/router';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { UpdateGiftComponent } from '../update-gift/update-gift.component';
import { TooltipModule } from 'primeng/tooltip';
import { NgStyle, NgClass, NgIf } from '@angular/common';
import { AsyncPipe } from '@angular/common';
import { InputIconModule } from 'primeng/inputicon';
import { IconField } from 'primeng/iconfield';
import { DialogModule } from 'primeng/dialog';
import { Dialog } from 'primeng/dialog';
import { ConfirmationService } from 'primeng/api';
import { NgFor } from '@angular/common';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-donator',
  standalone: true,
  imports: [RouterLink, CommonModule, NgFor, HttpClientModule, Dialog, DialogModule, IconField, InputIconModule, AsyncPipe, NgStyle, NgClass, NgIf, TooltipModule, UpdateGiftComponent, ConfirmDialogModule, RouterOutlet, ToastModule, ConfirmPopupModule, FormsModule, SelectButtonModule, ButtonModule, TableModule],
  templateUrl: './donator.component.html',
  styleUrl: './donator.component.css',
  providers: [ConfirmationService, Router, MessageService, HttpClientModule, HttpClient, DonatorsService, ServiceService]
})
export class DonatorComponent implements OnInit{

  constructor(private http: HttpClient, private messegeService: MessageService, private router: Router) { }

  donatorService: DonatorsService = inject(DonatorsService);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  giftService: ServiceService = inject(ServiceService);

  donatorList: Donator[] = [];
  first = 0;
  revenue: boolean = false;
  tr: number = 0;
  selectedDonatorId: number = 0;
  confirmDialogVisible: boolean = false;
  display: boolean = false;

  ngOnInit() {
    console.log('Component B Initialized');
    this.getDonators();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      console.log(event.url);

      if (event.url === this.router.url) {
        this.getDonators();
      }
    });
  }
  async getDonators() {
    try {
      this.donatorService.getAllDonators().subscribe((data: any) => {
        this.donatorList = data;
      });
    } catch {
      console.log("not athourizied");
      this.messegeService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
    }
  }

  prev() {
    this.first = Math.max(0, this.first - 5);
  }

  next() {
    this.first = this.first + 5;
  }

  reset() {
    this.first = 0;
  }

  isFirstPage() {
    return this.first === 0;
  }

  isLastPage() {
    return this.first >= this.donatorList.length - 5;
  }

  pageChange(event: any) {
    this.first = event.first;
  }

  deleteDonator(event: Event, donatorId: number) {
    if (this.giftService.getRole() != 'Admin')
      this.messegeService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה' });
    else {
      event.stopPropagation();
      this.confirmDialogVisible = true;
      this.selectedDonatorId = donatorId;
    }
  }

  async onConfirm() {
    this.confirmDialogVisible = false;
    this.donatorService.deleteDonator(this.selectedDonatorId).subscribe({
      next: (response) => {
        console.log('סטטוס:', response.status);
        console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
        this.showSuccessToast();
      },
      error: (error) => {
        console.error('שגיאה בביצוע הקריאה:', error);
        this.messegeService.add({ severity: 'error', summary: 'Error', detail: 'אין אפשרות למחוק את התורם!' });
        setTimeout(() => {
          this.messegeService.clear();
        }, 3000);
      }
    });
  }

  onReject() {
    this.confirmDialogVisible = false;
    console.log(this.confirmDialogVisible);
    this.messegeService.add({ severity: 'info', summary: 'Cancelled', detail: 'מחיקה נסגרה!' });
    setTimeout(() => {
      this.messegeService.clear();
    }, 1200);
  }

  async showSuccessToast() {
    this.messegeService.add({ severity: 'success', summary: 'Deleted', detail: 'תורם נמחק בהצלחה!' });
    setTimeout(() => {
      this.messegeService.clear();
      this.getDonators();
    }, 1200);
  }

  async updateDonator(id: number) {
    if (this.giftService.getRole() != 'Admin')
      this.messegeService.add({ severity: 'warn', summary: 'Cancelled', detail: 'משתמש לא מורשה' });
    else
      await this.router.navigate(['updateDonator/' + id], { relativeTo: this.activatedRoute })
  }

  async addDonator() {
    if (this.giftService.getRole() != 'Admin')
      this.messegeService.add({ severity: 'warn', summary: 'Cancelled', detail: 'משתמש לא מורשה' });
    else {
      console.log("before route");

      await this.router.navigate(['donator/addDonator'])
      console.log("after route");

    }
  }
  logOut() {
    localStorage.clear();
    this.router.navigate(['login'])
  }

  async winnersReport() {
    this.giftService.createWinnerReport().subscribe({
      next: (file: Blob) => {
        const fileName = 'רשימת זוכים.xlsx';
        const url = window.URL.createObjectURL(file);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('שגיאה בהורדת הקובץ', err);
      },
    });
  }

  async totalRevenue() {
    this.revenue = true;
    this.giftService.totalReveue().subscribe({
      next: (data: any) => {
        this.tr = data;
      },
      error: (err) => {
        console.log(err.error);
      }
    })
  }
  closeD() {
    this.revenue = false;
  }

  async removeWinners() {
    await this.winnersReport()
    this.giftService.removeWinners().subscribe({
      next: (response) => {
        console.log(response.body);
        this.messegeService.add({ severity: 'info', summary: 'Success', detail: 'רשימת הזוכים נמחקה' });
      },
      error: (err) => {
        console.log(err.error.text + " " + err.status);
        if (err.status === 200) {
          this.messegeService.add({ severity: 'info', summary: 'Success', detail: 'רשימת הזוכים נמחקה' });
        }
        else
          this.messegeService.add({ severity: 'error', summary: 'Error', detail: 'שגיאה במחיקת רשימת הזוכים' });
      }
    })
  }

  // gifts() {
  //   this.router.navigateByUrl('/admin')
  // }
}
