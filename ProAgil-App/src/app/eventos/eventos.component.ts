import { Evento } from './../_models/Evento';
import { EventoService } from './../_services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  titulo = 'Eventos';

  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;

  modosalvar = 'post';
  bodyDeletarEvento = '';

  file: File;

  imglargura = 50;
  imgaltura = 2;
  imgmostrar = false;
  registerForm: FormGroup;

  fileNameToUpdate = '';

  dataAtual: string;

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
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeservice: BsLocaleService,
    private toastr: ToastrService
    ) {
      this.localeservice.use('pt-br');
    }

  openModal(template: any)
  {
    this.registerForm.reset();
    template.show();
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  validation(){
    this.registerForm = this.fb.group({
      tema: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(50)]],
      local: ['',Validators.required],
      dataEvento: ['',Validators.required],
      imagemURL: ['',Validators.required],
      qtdPessoas: ['',[Validators.required,Validators.max(120000)]],
      telefone: ['',Validators.required],
      email: ['',[Validators.required,Validators.email]]
    });
  }

  editarEvento(evento: Evento, template: any)
  {
    this.modosalvar = 'put';
    this.openModal(template);
    this.evento = Object.assign({}, evento);
    this.fileNameToUpdate = evento.imagemURL.toString();
    this.evento.imagemURL = '';
    this.registerForm.patchValue(this.evento);
  }

  novoEvento(template: any){
    this.modosalvar = 'post';
    this.openModal(template);
  }

  onFileChange(event){
    const reader = new FileReader();
    if(event.target.files && event.target.files.length){
      this.file = event.target.files;
    }

  }

  uploadImagem()
  {
    if (this.modosalvar === 'post')
    {
    const nomeArquivo = this.evento.imagemURL.split('\\', 3);
    this.evento.imagemURL = nomeArquivo[2];
    this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe(
      () => {
        this.dataAtual = new Date().getMilliseconds().toString();
        this.getEventos();
      }

    );
    } else{
      this.evento.imagemURL = this.fileNameToUpdate;
      this.eventoService.postUpload(this.file, this.fileNameToUpdate).subscribe(
        () => {
          this.dataAtual = new Date().getMilliseconds().toString();
          this.getEventos();
        }

      );
    }
  }

  salvarAlteracao(template: any)
  {
    if (this.registerForm.valid)
    {
      if (this.modosalvar === 'post')
      {
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.postEvento(this.evento).subscribe(
          (novoevento: Evento) => {
            template.hide();
            this.getEventos();
            this.toastr.success('Inserido com Sucesso!');
          }, error => {
            this.toastr.error(`Erro Ao Inserir: ${error}`);
            //console.log(error);
          }
        );
      } else {
        this.evento = Object.assign({id : this.evento.id }, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.putEvento(this.evento).subscribe(
          (novoevento: Evento) => {
            template.hide();
            this.getEventos();
            this.toastr.success('Alterado com Sucesso!');
          }, error => {
            this.toastr.error(`Erro Ao Alterar: ${error}`);
            //console.log(error);
          }
        );
      }

    }
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
    }, error => {
      this.toastr.error(`Erro Ao Carregar: ${error}`);
      //console.log(error);
    });
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Deletado com Sucesso!');
        }, error => {
          this.toastr.error('Erro ao Deletar!');
          console.log(error);
        }
    );
  }

}
