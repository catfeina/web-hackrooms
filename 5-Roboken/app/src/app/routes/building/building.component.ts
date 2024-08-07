import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskResponse } from '../../interfaces/Task';
import { ApiService } from '../../services/api.service';
import { ApiResponse } from '../../interfaces/Api';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-building',
  templateUrl: './building.component.html',
  styleUrl: './building.component.css'
})
export class BuildingComponent implements OnInit {
  constructor(
    private _me: ActivatedRoute,
    private _route: Router,
    private _api: ApiService,
    private _role: RoleService
  ) { }
  ngOnInit(): void {
    this._me.params.subscribe(p => {
      const id = p["Id"];

      if (id) {
        this._taskCode = Number(id);
        this.GetTask(this._taskCode);
      } else {
        this._route.navigate(['/street']);
      }
    });

    switch (this._role.GetRole()) {
      case "Lvl1":
        this.lblActionByRole = 'Comentar';
        break;
      case "Lvl2":
        this.lblActionByRole = 'Aprobar';
        break;
      case "Lvl3":
        this.lblActionByRole = 'Cerrar';
        break;
      default:
        this.lblActionByRole = 'Action';
        break;
    }
  }

  private _taskCode: number = 0;
  lblActionByRole: string = '';
  txtComment: string = '';
  txtMessage: string = '';
  task: TaskResponse | null = null;

  GetTask(
    taskCode: number
  ) {
    this._api.Get<TaskResponse>(`Task/${taskCode}`).subscribe(
      response => {
        this.task = response;
      }, error => {
        console.log(error);
      }
    );
  }

  CommentTask(
    endpoint?: string
  ) {
    this.txtComment = this.txtComment.trim();

    if (this.txtComment == '') {
      this.txtMessage = 'No olvide ingresar su comentario';
      return;
    }

    if (!endpoint) {
      endpoint = 'Comment';
    }

    const request = {
      taskCode: this._taskCode,
      comment: this.txtComment,
      status: this.task?.status
    };

    this._api.Post<ApiResponse>(`Task/${endpoint}`, request).subscribe(
      response => {
        this.txtMessage = response.message;

        if (response.success) {
          this.GetTask(this._taskCode);
        }
      }, error => {
        console.log('[+] Error al comentar tarea: ', error);
      }
    );

    this.txtComment = '';
  }

  ActionByRole() {
    switch (this._role.GetRole()) {
      case 'Lvl1':
        this.CommentTask('Pending');
        break;
      case 'Lvl2':
        this.CommentTask('Approve');
        break;
      case 'Lvl3':
        this.CommentTask('Close');
        break;
      default:
        this.CommentTask();
        break;
    }
  }
}
