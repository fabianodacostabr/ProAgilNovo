import { Evento } from './../_models/Evento';
import { EventoService } from './../_services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventosFiltrados: Evento[];
  eventos: Evento[];
  imglargura = 50;
  imgaltura = 2;
  imgmostrar = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;

  _filtrolista: string;
  get filtrolista(): string{
    return this._filtrolista;
  }
  set filtrolista(value: string){
    this._filtrolista = value;
    this.eventosFiltrados = this.filtrolista ? this.filtrarEvento(this.filtrolista) : this.eventos;
  }


  /*eventos: any = [
    {
      EventoId: 1,
      Tema: 'Angular',
      Local: 'São Paulo'
    },
    {
      EventoId: 2,
      Tema: '.Net',
      Local: 'Dracena'
    },
    {
      EventoId: 3,
      Tema: 'SQL',
      Local: 'São Paulo'
    }
  ];*/


  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
    ) { }

  openModal(template: TemplateRef<any>)
  {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  validation(){
    this.registerForm = new FormGroup({
      tema: new FormControl('',
      [Validators.required,Validators.minLength(4),Validators.maxLength(50)]),
      local: new FormControl('',Validators.required),
      dataEvento: new FormControl('',Validators.required),
      imagemURL: new FormControl('',Validators.required),
      qtdPessoas: new FormControl('',[Validators.required,Validators.max(120000)]),
      telefone: new FormControl('',Validators.required),
      email: new FormControl('',[Validators.required,Validators.email])
    });
  }

  salvarAlteracao()
  {

  }

  alterarImagem()
  {
    this.imgmostrar = !this.imgmostrar;
  }

  filtrarEvento(filtro:string) : Evento[]
  {
    filtro = filtro.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtro) !== -1
    );

  }

  getEventos()
  {
    this.eventoService.getAllEvento().subscribe((_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos);
    }, error => {
      console.log(error);
    });
  }

}
