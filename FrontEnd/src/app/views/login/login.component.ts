import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup} from '@angular/forms';
import { ILoginResponse } from '../../Models/ILoginResponse';
import { UserService } from '../../Services/UserService';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit
{
  formLogin = new FormGroup({
    email: new FormControl,
    password: new FormControl
  });

  ngOnInit(): void 
  {

  }

  
  constructor(private post: UserService) {
    this.post.getUsers().subscribe((data) => {
      console.log(data);
    });
   }
   

  OnLogin(form:any){
    console.log(form);
    this.post.Login(form).subscribe((data) => {
      console.log(data);
    });
  }
}
