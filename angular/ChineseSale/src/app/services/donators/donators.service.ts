import { Injectable } from '@angular/core';
import { Donator } from '../../models/donator.model'
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { HttpResponse } from '@angular/common/http';
import { map } from 'rxjs';
import { catchError } from 'rxjs';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DonatorsService {

  constructor(private http: HttpClient) { }

  getAllDonators(): Observable<Donator[]> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Donator[]>("https://localhost:7282/api/Donator", { headers })
  }

  deleteDonator(id: number) {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.delete("https://localhost:7282/api/Donator/" + id, { headers, observe: 'response', responseType: 'text' });
  }

  addDonator(donator: Donator): Observable<any> {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.post<any>("https://localhost:7282/api/Donator", donator, { headers, observe: 'response' })
      .pipe(
        map((response: HttpResponse<any>) => {
          const responseBody = response.body;
          return {
            status: response.status,
            message: responseBody.message,
          };
        }),
        catchError((error) => {
          console.log("error service", error.status);
          return throwError(error);
        })
      );
  }


  updateDonator(donator: Donator) {
    var token = this.getToken();
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.put("https://localhost:7282/api/Donator", donator, { headers, observe: 'response', responseType: 'text' });
  }


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
}
