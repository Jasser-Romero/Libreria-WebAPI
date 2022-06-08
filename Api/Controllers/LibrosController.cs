using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModels;

namespace Api.Controllers
{
    public class LibrosController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<LibrosViewModel> list = new List<LibrosViewModel>();

            using(LibreriaEntities1 db = new LibreriaEntities1())
            {
                list = (from x in db.Libros
                        select new LibrosViewModel
                        {
                            Id = x.Id,
                            Autor = x.Autor,
                            Editorial = x.Editorial,
                            ISBN = x.ISBN,
                            Temas = x.Temas,
                            Titulo = x.Titulo
                        }).ToList();
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            LibrosViewModel libro = null;

            using(LibreriaEntities1 db = new LibreriaEntities1())
            {
                libro = db.Libros.Where(x => x.Id == id).Select(x => new LibrosViewModel()
                {
                    Id = x.Id,
                    Autor = x.Autor,
                    Editorial = x.Editorial,
                    ISBN = x.ISBN,
                    Temas = x.Temas,
                    Titulo = x.Titulo
                }).FirstOrDefault();
            }

            if (libro == null)
                return NotFound();

            return Ok(libro);
        }

        [HttpPost]
        public IHttpActionResult Add(LibrosViewModel model)
        {
            using(LibreriaEntities1 db = new LibreriaEntities1())
            {
                var oLibro = new Libros()
                {
                    ISBN = model.ISBN,
                    Autor = model.Autor,
                    Editorial = model.Editorial,
                    Temas = model.Temas,
                    Titulo = model.Titulo
                };
                db.Libros.Add(oLibro);
                db.SaveChanges();
            }
            return Ok("Registro agregado correctamente");
        }

        [HttpPut]
        public IHttpActionResult Put(LibrosViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("No es un modelo valido");

            using(LibreriaEntities1 db = new LibreriaEntities1())
            {
                var oLibro = db.Libros.Where(l => l.Id == model.Id)
                    .FirstOrDefault<Libros>();
                if (oLibro != null)
                {
                    oLibro.ISBN = model.ISBN;
                    oLibro.Autor = model.Autor;
                    oLibro.Editorial = model.Editorial;
                    oLibro.Temas = model.Temas;
                    oLibro.Titulo = model.Titulo;
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Ok("simon");
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("No es un id de libro válido");

            using(LibreriaEntities1 db = new LibreriaEntities1())
            {
                var libro = db.Libros.Where(x => x.Id == id)
                    .FirstOrDefault();
                db.Entry(libro).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            return Ok();
        }
    }
}
