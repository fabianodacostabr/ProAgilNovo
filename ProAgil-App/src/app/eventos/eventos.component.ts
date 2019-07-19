import { Evento } from './../_models/Evento';
import { EventoService } from './../_services/evento.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  _filtrolista: string;
  get filtrolista(): string{
    return this._filtrolista;
  }
  set filtrolista(value: string){
    this._filtrolista = value;
    this.eventosFiltrados = this.filtrolista ? this.filtrarEvento(this.filtrolista) : this.eventos;
  }

  eventosFiltrados: Evento[];
  eventos: Evento[];
  imglargura = 50;
  imgaltura = 2;
  imgmostrar = false;


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

  constructor(private eventoService: EventoService) { }

  ngOnInit() {
    this.getEventos();
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
