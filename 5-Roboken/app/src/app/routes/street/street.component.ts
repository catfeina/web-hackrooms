import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ApiResponse } from '../../interfaces/Api';
import { TaskResponse } from '../../interfaces/Task';
import { Router } from '@angular/router';

@Component({
  selector: 'app-street',
  templateUrl: './street.component.html',
  styleUrl: './street.component.css'
})
export class StreetComponent implements OnInit {
  constructor(
    private _api: ApiService,
    private _route: Router
  ) { }
  ngOnInit(): void {
    this.GetTasks();
  }

  txtTitle: string = '';
  txtDescription: string = '';
  message: string = '';
  tasks: TaskResponse[] = [];

  AddTask() {
    const title = this.txtTitle;
    const description = this.txtDescription;

    if (title == '') {
      this.message = 'Debe ingresar un título';
      return;
    }

    if (description == '') {
      this.message = 'Debe ingresar una descripción';
      return;
    }

    this._api.Post<ApiResponse>('Task/Create', { title, description }).subscribe(
      response => {
        this.message = response.message;
        this.GetTasks();
      }, error => {
        console.log('[+] Error al crear tarea: ', error);
      }
    );
  }

  GetTasks() {
    this._api.Get<TaskResponse[]>('Task').subscribe(
      response => {
        this.tasks = response;
      }, error => {
        this.message = 'Ocurrió un error al traer las tareas :c';
        console.log('[+] Error al leer tareas: ', error);
      }
    );
  }

  ShowTask(
    taskCode: number
  ) {
    this._route.navigate(['/building', taskCode]);
  }
}
