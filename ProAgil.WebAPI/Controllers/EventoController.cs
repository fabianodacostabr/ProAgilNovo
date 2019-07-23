using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using ProAgil.Domain;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public EventoController(IProAgilRepository repo){
            _repo = repo;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try{
                var results = await _repo.GetAllEventosAsync(true);
                return Ok(results);
            }
            catch(Exception err)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }
        }

        
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
             try{
                var resultado = await _repo.GetAllEventoAsyncById(Id,true);
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
                var resultado = await _repo.GetAllEventosAsyncByTema(tema,true);
                return Ok(resultado);
             }
             catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }
        }



        // POST 
        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
             try{
               _repo.Add(model);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }                
             }
             catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha DB");
            }

            return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int Id, Evento model)
        {
            try{
                var evento = await _repo.GetAllEventoAsyncById(Id, false);
                if(evento == null){
                    return NotFound();
                }

               _repo.Update(model);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", model);
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
