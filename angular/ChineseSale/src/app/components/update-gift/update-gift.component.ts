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
    selector: 'app-update-gift',
    standalone: true,
    imports: [ConfirmDialog, ToastModule, HttpClientModule, DialogModule, ButtonModule, NgIf, ReactiveFormsModule, CommonModule, FileUploadModule],
    templateUrl: './update-gift.component.html',
    styleUrl: './update-gift.component.css',
    providers: [MessageService, ServiceService, HttpClient, DonatorsService]
})
export class UpdateGiftComponent {
    constructor(private cdr: ChangeDetectorRef,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private messageService: MessageService
    ) {
        this.frmGift = new FormGroup({
            name: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]),
            donator: new FormControl(0, Validators.required),
            price: new FormControl(0, [Validators.required, Validators.min(1), Validators.max(100)]),
            category: new FormControl(0, Validators.required)
        })
    }

    frmGift: FormGroup;

    giftId: number = 0;
    giftList: Gift[] = [];
    giftById: any | null = null;

    defaultName: string = '';
    defaultPrice: number = 10;
    defaultPicture: string | ArrayBuffer | null = null;
    defaultCategory: Category | null = null;

    selectedFile: File | null = null;
    visible: boolean = true;

    donatorService: DonatorsService = inject(DonatorsService);
    giftService: ServiceService = inject(ServiceService);

    donatorList: Donator[] = [];
    categoryList: Category[] = [];

    ngOnInit() {
        this.activatedRoute.params.subscribe(params => {
            if (params['id'] && params['id'] > 0) {
                this.giftId = params['id'];
                console.log(this.giftId);
                this.data();
            }
        });
    }

    data() {
        this.selectGiftList();
    }

    selectGiftList() {
        this.giftService.getAllGifts().subscribe((data: any) => {
            this.giftList = data;
            this.selectDonatorList();
        });
    }

    selectDonatorList() {
        this.donatorService.getAllDonators().subscribe((data: Donator[]) => {
            this.donatorList = data;
            this.giftService.getAllCategories().subscribe((data: any) => {
                this.categoryList = data;
                console.log(this.giftList);
                this.findGift();
            });

            this.cdr.detectChanges(); // הוסף כאן כדי לודא ש-Angular יודע על השינויים
        });
    }

    findGift() {
        this.giftList.forEach(element => {
            if (element.id == this.giftId)
                this.giftById = element;
        });

        if (this.giftById) {
            this.frmGift.controls['name'].setValue(this.giftById.name);
            this.frmGift.controls['price'].setValue(this.giftById.price);
            this.frmGift.controls['category'].setValue(this.giftById.category.id);
            this.frmGift.controls['donator'].setValue(this.giftById.donatorId);
            this.defaultPicture = this.giftById.picture;
        }
    }

    async updateGift(event: Event) {
        event.preventDefault();

        const gift = new Gift();
        gift.id = this.giftId;
        gift.name = this.frmGift.controls['name'].value;
        gift.donatorId = this.frmGift.controls['donator'].value;
        gift.price = this.frmGift.controls['price'].value;
        gift.countOfSales = this.giftById.countOfSales;
        gift.picture = this.giftById.picture;
        gift.status = this.giftById.status;
        gift.categoryId = this.frmGift.controls['category'].value;
        gift.category = this.giftById.category;

        console.log(gift.name + ": " + gift.donatorId + " " + gift.category.title);
        this.giftService.updateGift(gift).subscribe({
            next: async response => {
                console.log('סטטוס:', response.status);
                console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
                if (this.selectedFile) {
                    await this.giftService.updateImage(this.selectedFile, this.giftId);
                }
                await this.showSuccessToast();
            },
            error: error => {
                console.error('שגיאה בעדכון מתנה:', error);
                this.messageService.add({ severity: 'error', summary: 'Error', detail: 'מתנה לא עודכנה!' });
                setTimeout(() => {
                  this.messageService.clear(); // מנקה את כל הודעות הטוסט
                }, 3000);
            }
        });
    }
    async showSuccessToast() {
        console.log("visible");
        this.visible = false;
        this.messageService.add({ severity: 'success', summary: 'update', detail: 'מתנה עודכנה בהצלחה!' });
        this.close();
    }

    close() {
        setTimeout(() => {
            this.messageService.clear();
            this.route();
        }, 2000);
    }

    route(){
        this.router.navigate(['admin']);
    }

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.selectedFile = input.files[0];

            // יצירת תצוגה מקדימה של התמונה
            const reader = new FileReader();
            reader.onload = () => {
                this.defaultPicture = reader.result; // שומר את התמונה במשתנה
            };
            reader.readAsDataURL(this.selectedFile);
        }
    }
}
