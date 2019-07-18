import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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

  eventosFiltrados: any = [];
  eventos: any =[];
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

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEventos();
  }

  alterarImagem()
  {
    this.imgmostrar = !this.imgmostrar;
  }

  filtrarEvento(filtro:string) : any
  {
    filtro = filtro.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtro) !== -1
    );

  }

  getEventos()
  {
    this.eventos = this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.eventos = response;
    }, error => {
      console.log(error);
    }    
    );
  }

}
