using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using ProAgil.Domain;
using AutoMapper;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        public readonly IMapper _mapper;

        public EventoController(IProAgilRepository repo, IMapper mapper){
            _repo = repo;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try{
                var eventos = await _repo.GetAllEventosAsync(true);
                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch(Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,$"Falha DB: {err.ToString()}");
            }
        }

        
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
             try{
                var evento = await _repo.GetAllEventoAsyncById(Id,true);

                var resultado = _mapper.Map<EventoDto>(evento);

                return Ok(resultado);
             }
             catch(Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
             try{
                var eventos = await _repo.GetAllEventosAsyncByTema(tema,true);
                var results = _mapper.Map<EventoDto[]>(eventos);

                return Ok(results);
             }
             catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }
        }



        // POST 
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
             try{

                var evento = _mapper.Map<Evento>(model);

                _repo.Add(evento);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }                
             }
             catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }

            return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int Id, EventoDto model)
        {
            try{

                var evento = await _repo.GetAllEventoAsyncById(Id, false);
                if(evento == null){
                    return NotFound();
                }

                _mapper.Map(model, evento);

               _repo.Update(model);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }                
             }
             catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try{
                var evento = await _repo.GetAllEventoAsyncById(Id, false);
                if(evento == null){
                    return NotFound();
                }

               _repo.Delete(evento);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }                
             }
             catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }

            return BadRequest();
        }


    }
}
