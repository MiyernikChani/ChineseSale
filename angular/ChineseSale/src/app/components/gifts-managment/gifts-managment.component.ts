import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../services/gifts/gifts.service';
import { Gift } from '../../models/gift.model';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule } from '@angular/forms';
import { inject } from '@angular/core';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ToastModule } from 'primeng/toast';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DonatorsService } from '../../services/donators/donators.service';
import { UpdateGiftComponent } from '../update-gift/update-gift.component';
import { TooltipModule } from 'primeng/tooltip';
import { NgStyle, NgClass, NgIf } from '@angular/common';
import { AsyncPipe } from '@angular/common';
import { InputIconModule } from 'primeng/inputicon';
import { IconField } from 'primeng/iconfield';
import { NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';
import { DialogModule } from 'primeng/dialog';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-gifts-managment',
  standalone: true,
  imports: [RouterLink, DialogModule, HttpClientModule, IconField, InputIconModule, AsyncPipe, NgIf, NgClass, NgStyle, TooltipModule, UpdateGiftComponent, ConfirmDialogModule, RouterOutlet, ToastModule, FormsModule, TableModule, ButtonModule, SelectButtonModule, ConfirmPopupModule], // הוספת המודול כאן
  templateUrl: './gifts-managment.component.html',
  styleUrls: ['./gifts-managment.component.css'],
  providers: [HttpClientModule, MessageService, ConfirmationService, ServiceService, HttpClient, DonatorsService]
})
export class GiftsManagmentComponent implements OnInit {
  constructor(private messageService: MessageService, private confirmationService: ConfirmationService) { }
  giftService: ServiceService = inject(ServiceService);
  donatorService: DonatorsService = inject(DonatorsService);
  giftList: Gift[] = [];
  namesList$: { [key: number]: string } = {};
  first = 0;
  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  selectedGiftId: number = 0;
  confirmDialogVisible: boolean = false;
  display: boolean = false;

  randomUser: string = '';
  randomGift: Gift | null = null;

  revenue: boolean = false;
  tr: number = 0;

  imageLoaded: boolean = false;
  loadImage(product: any) {
    const img = new Image();
    img.src = 'assets/images/' + product.picture;  // נתיב התמונה

    img.onload = () => {
      this.imageLoaded = true;  // אם התמונה נטענה בהצלחה
    };

    img.onerror = () => {
      this.imageLoaded = false;  // אם התמונה לא הצליחה לטעון
    };
  }

  ngOnInit() {
    console.log('Component A Initialized');
    this.selectGiftList();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      console.log(event.url);
      if (event.url === this.router.url) {
        this.selectGiftList();
      }
    });
  }

  selectGiftList() {
    try {
      this.giftService.getAllGifts().subscribe((data: any) => {
        this.giftList = data;
        this.giftList.forEach(product => {
          this.loadImage(product);  // טוען את התמונה
        });
        this.getDonators();
      });
    } catch {
      console.log("not athourizied");
      this.messageService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
    }
    console.log(this.giftList);
    console.log(this.namesList$);
  }


  async getDonators() {
    const promises = this.giftList.map(async (d) => {
      try {
        const donatorName = await this.getDonatorByGiftId(d.id);
        if (donatorName) {
          this.namesList$[d.id] = donatorName;
        }
      } catch (error) {
        console.error('שגיאה בקבלת שם התורם:', error);
      }
    });

    await Promise.all(promises);
  }

  getDonatorByGiftId(id: number) {
    return this.giftService.getDonatorByGiftId(id)
      .then(n => n || "")
      .catch(error => {
        console.error('שגיאה בקבלת שם התורם:', error);
        return "";
      });
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
    return this.first >= this.giftList.length - 5;
  }

  pageChange(event: any) {
    this.first = event.first;
  }

  deleteGift(event: Event, giftId: number) {
    if (this.giftService.getRole() != 'Admin')
      this.messageService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה' });
    else {
      event.stopPropagation();
      this.confirmDialogVisible = true;
      this.selectedGiftId = giftId;
    }
  }

  async onConfirm() {
    this.confirmDialogVisible = false;
    this.giftService.deleteGift(this.selectedGiftId).subscribe({
      next: (response) => {
        console.log('סטטוס:', response.status);
        console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
        this.showSuccessToast();
      },
      error: (error) => {
        console.error('שגיאה בביצוע הקריאה:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'אין אפשרות למחוק את המתנה!' });
        setTimeout(() => {
          this.messageService.clear();
        }, 3000);
      }
    });
  }

  onReject() {
    this.confirmDialogVisible = false;
    console.log(this.confirmDialogVisible);
    this.messageService.add({ severity: 'info', summary: 'Cancelled', detail: 'מחיקה נסגרה!' });
    setTimeout(() => {
      this.messageService.clear();
    }, 1200);
  }

  async showSuccessToast() {
    this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'מתנה נמחקה בהצלחה!' });
    setTimeout(() => {
      this.messageService.clear();
      this.selectGiftList();
    }, 1200);
  }

  async updateGift(id: number) {
    if (this.giftService.getRole() != 'Admin')
      this.messageService.add({ severity: 'warn', summary: 'Cancelled', detail: 'משתמש לא מורשה' });
    else
      await this.router.navigate(['update/' + id], { relativeTo: this.activatedRoute })
  }

  async addGift() {
    if (this.giftService.getRole() != 'Admin')
      this.messageService.add({ severity: 'warn', summary: 'Cancelled', detail: 'משתמש לא מורשה' });
    else
      await this.router.navigate(['add'], { relativeTo: this.activatedRoute })
  }

  logOut() {
    localStorage.clear();
    this.router.navigate(['login'])
  }

  random(id: number) {
    this.giftList.forEach((item => {
      if (item.id === id)
        this.randomGift = item;
      console.log(this.randomGift?.name);
    }))

    this.giftService.random(id).subscribe({
      next: (data: any) => {
        this.randomUser = data;
        this.startRaffle();
      },
      error: (err) => {
        if (err.status === 200) {
          this.randomUser = err.error.text.substring(15);
          console.log(err.error.text);
          this.startRaffle();
        }
        else if (err.status === 404) {
          console.log("אין רכישות" + err.error);
          this.messageService.add({ severity: 'warn', summary: 'warning', detail: 'אין רכישות למתנה זו!' });
        }
        else if (err.status === 400) {
          console.log("לא ניתן להגריל תורם נוסף!" + err.error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'מתנה כבר הוגרלה!' });
        }
        else {
          console.log(err.error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'שגיאה בהגרלת מתנה' });
        }
      }
    });

  }

  startRaffle() {
    this.display = true;
  }

  closeDialog() {
    this.display = false;
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

  removeWinners() {
    this.giftService.removeWinners().subscribe({
      next: (response) => {
        console.log(response.body);
        this.messageService.add({ severity: 'info', summary: 'Success', detail: 'רשימת הזוכים נמחקה' });
      },
      error: (err) => {
        console.log(err.error.text + " " + err.status);
        if (err.status === 200) {
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'רשימת הזוכים נמחקה' });
        }
        else
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'שגיאה במחיקת רשימת הזוכים' });
      }
    })
  }

  donators() {
    this.router.navigateByUrl('donator')
  }

  buyersDetails(giftId:number){
    this.router.navigate(['admin/buyersDetails', giftId]);
  }
}
