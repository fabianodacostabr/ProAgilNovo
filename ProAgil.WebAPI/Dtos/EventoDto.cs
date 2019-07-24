
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Local � Obrigat�rio")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Local � entre 3 e 100 caracteres")]
        public string Local { get; set; }
        public string  DataEvento { get; set; }

        [Required(ErrorMessage ="O Tema deve ser Preenchido")]
        public string Tema { get; set; }
        [Range(2,12000, ErrorMessage = "Quantidade Excedida")]
        public int QtdPessoas { get; set; }       
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }

        public List<PalestranteDto> Palestrantes { get; set; }
    }
}