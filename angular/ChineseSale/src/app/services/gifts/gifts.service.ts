import { Injectable } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { DonatorsService } from '../donators/donators.service';
import { inject } from '@angular/core';
import { Donator } from '../../models/donator.model';
import { Category } from '../../models/category.model';
import { HttpClient } from '@angular/common/http';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs';
import { catchError } from 'rxjs';
import { throwError } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { UserService } from '../user/user.service';
import { Inject } from '@angular/core';
import { Purchase } from '../../models/puchase.model';
import { User } from '../../models/user.model';
import { tap } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class ServiceService {
  constructor(private http: HttpClient) { }
  userService: UserService = Inject(UserService)

  ngOnInit() {
    this.donatorService.getAllDonators().subscribe((data: Donator[]) => {
      this.donatorsList = data;
    })
  }

  donatorService: DonatorsService = inject(DonatorsService);
  donatorsList: Donator[] = [];

  categoryList: Category[] = [{ id: 1, title: "big gift" },
  { id: 2, title: "small gift" }
  ];

  getToken() {
    const userDataString = localStorage.getItem('userData');
    let userData;
    var token = "";
    if (userDataString) {
      userData = JSON.parse(userDataString);
      token = userData.token;
    }
    return token;
  }

  getRole() {
    const userDataString = localStorage.getItem('userData');
    let userData;
    var role = "";
    if (userDataString) {
      userData = JSON.parse(userDataString);
      role = userData.role;
    }
    return role;
  }

  getItems(): Observable<Gift[]> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Gift[]>('https://localhost:7282/api/Gift', { headers });
  }

  getAllGifts() {
    return this.getItems();
  }

  deleteGift(id: number) {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete("https://localhost:7282/api/Gift/" + id, { headers, observe: 'response', responseType: 'text' });
  }

  updateGift(gift: Gift) {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put("https://localhost:7282/api/Gift", gift, { headers, observe: 'response', responseType: 'text' });
  }

  updateImage(image: File | null = null, id: number) {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    const formData = new FormData();
    if (image) {
      formData.append('file', image);
      console.log("image not null");
    }
    return this.http.post(`https://localhost:7282/api/Gift/uploadGiftImage?giftId=${id}`, formData, { headers, observe: 'response', responseType: 'text' }).subscribe({
      next: (response) => {
        console.log('סטטוס:', response.status);
        console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
      },
      error: (error) => {
        console.error('שגיאה בהוספת התמונה:', error);
      }
    });
  }

  addGift(gift: Gift): Observable<any> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post<any>("https://localhost:7282/api/Gift", gift, { headers, observe: 'response' })
      .pipe(
        map((response: HttpResponse<any>) => {
          const responseBody = response.body;
          const giftData = responseBody.gift ? responseBody.gift : null;
          return {
            status: response.status,
            message: responseBody.message,
            gift: giftData,
          };
        }),
        catchError((error) => {
          console.log("error service", error.status);
          return throwError(error);
        })
      );
  }

  getAllCategories() {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Gift[]>("https://localhost:7282/api/Gift/getAllCategories", { headers })
  }

  getDonatorByGiftId(id: number): Promise<string> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return new Promise((resolve, reject) => {
      this.http.get<Donator>(`https://localhost:7282/api/Gift/getDonatorByGiftId/${id}`, { headers }).subscribe({
        next: donator => {
          resolve(donator.name);
        },
        error: error => {
          console.error('שגיאה בקבלת תורם:', error);
          reject(error);
        }
      });
    });
  }

  random(id: number): Observable<User> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<User>(`https://localhost:7282/api/Random/api/Random/randomWin/${id}`, { headers });
  }

  createWinnerReport(): Observable<Blob> {
    const token = this.getToken();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.get("https://localhost:7282/api/Random/api/Random/createWinnerReport", {
      headers,
      responseType: 'blob', // כדי לקבל את התגובה כקובץ
    });
  }

  totalReveue(): Observable<number> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<number>("https://localhost:7282/api/Random/api/Random/getTotalRevenue", { headers });
  }
  removeWinners(): Observable<Response> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Response>("https://localhost:7282/api/Random/api/Gift/MakeAllGiftsAvailable", { headers });
  }

  getAllPurchasesByGiftId(giftId: number): Observable<Purchase[]> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Purchase[]>(`https://localhost:7282/api/Purchase/api/Gift/getPurchasesByGiftId/${giftId}`, { headers });
  }

  getGiftById(id: number): Observable<Gift | null> {
    return this.getAllGifts().pipe(
      map((giftList: Gift[]) => {
        const gift = giftList.find(product => product.id === id);
        if (!gift) {
          console.error(`Gift with ID ${id} not found`);
          return null;
        }
        return gift;
      }),
      catchError((error) => {
        console.error('Error fetching gift:', error);
        return throwError(() => error);
      })
    );
  }  
}

