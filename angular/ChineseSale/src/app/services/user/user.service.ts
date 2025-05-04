import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../../models/user.model';
import { Observable, map, catchError, throwError } from 'rxjs';
import { HttpResponse } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Purchase } from '../../models/puchase.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }
  userToken: string = "";
  userRole: string = "";

  getToken() {
    const userDataString = localStorage.getItem('userData');
    let userData;
    var token = "";
    if (userDataString) {
      userData = JSON.parse(userDataString);
      token = userData.token;
    }
    console.log(token);

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

  login(firstName: string, lastName: string, password: string): Promise<string> {
    return new Promise((resolve, reject) => {
      this.http.post<{ token: string, role: string }>(`https://localhost:7282/api/Auth/login?firstName=${firstName}&lastName=${lastName}&password=${password}`, { observe: 'response' })
        .subscribe({
          next: (response) => {
            if (response && response.token && response.role) {
              this.userToken = response.token;
              this.userRole = response.role;
              console.log('הטוקן:', this.userToken);
              console.log('ההרשאה:', this.userRole);
              localStorage.setItem('userData', JSON.stringify({ token: this.userToken, role: this.userRole }));
              resolve('success');
            } else {
              console.error("משתמש לא מורשה");
              reject("משתמש לא מורשה");
            }
          },
          error: (error) => {
            console.error('שגיאה בהרשמה:', error.error ? error.error : error.message);
            reject(error.error ? error.error : error.message);
          }
        });
    });
  }

  register(user: User) {
    return this.http.post<any>("https://localhost:7282/api/Auth/register", user, { observe: 'response' });
  }

  async addToCart(giftId: number, sum: number) {
    var token = this.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    console.log("token: " + token);

    return new Promise((resolve, reject) => {
      this.http.post<any>(`https://localhost:7282/api/Customer/addGiftToCart?giftId=${giftId}&ammount=${sum}`, null, { headers, observe: 'response' }).subscribe({
        next: Response => {
          console.log("מתנה נוספה לסל בהצלחה!" + Response.status);
          resolve(Response.status);
        },
        error: error => {
          console.error('שגיאה בהוספה לסל:', error.body);
          reject(error);
        }
      });
    });
  }

  showBaskate(): Observable<Purchase[]> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Purchase[]>("https://localhost:7282/api/Customer/getShoppingCart", { headers });
  }

  historyShopping(): Observable<Purchase[]> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Purchase[]>("https://localhost:7282/api/Customer/shoppingHistory", { headers });
  }

  getPrice(): Observable<number> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<number>("https://localhost:7282/api/Customer/totalPrice", { headers });
  }

  buyCart() {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put("https://localhost:7282/api/Customer/buyCart", null, { headers, observe: 'response', responseType: 'text' });
  }

  getWinners(): Observable<Purchase[]> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Purchase[]>("https://localhost:7282/api/Customer/getAllWinners", {headers})
  }
}
