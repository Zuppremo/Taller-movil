import { Injectable } from '@angular/core';
import { ILoginRequest } from '../Models/ILoginRequest';
import { ILoginResponse } from '../Models/ILoginResponse';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  url:string = "https://localhost:7004/api/User/Login";

    constructor(private http: HttpClient) {}

    getUsers(){
        let url = "https://localhost:7004/api/User";
        return this.http.get(url);
    }

    Login (form:ILoginRequest): Observable<ILoginResponse>{
        const headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
        const json = JSON.stringify(form);
        return this.http.post<ILoginResponse>(this.url, json, {headers: headers});
    }
}
