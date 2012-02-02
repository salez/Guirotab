using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaSlideRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVaSlide> GetProdutoVaSlides()
        {
            var produtoVaSlides = from e in db.ProdutoVaSlides
                         select e;

            return produtoVaSlides;
        }

        public ProdutoVaSlide GetProdutoVaSlide(int id)
        {
            var produtoVaSlides = from e in db.ProdutoVaSlides
                         select e;

            return produtoVaSlides.SingleOrDefault(e => e.Id == id);
        }

        public ProdutoVaSlide GetProdutoVaSlideFarmacia(int idVa)
        {
            var produtoVaSlides = from e in db.ProdutoVaSlides
                                  where e.ProdutoVa.Id == idVa
                                  select e;

            return produtoVaSlides.ToList().Where(e => e.IsFarmacia()).FirstOrDefault();
        }

        public void Add(ProdutoVaSlide produtoVaSlide)
        {
            produtoVaSlide.DataInclusao = DateTime.Now;

            db.ProdutoVaSlides.InsertOnSubmit(produtoVaSlide);
        }

        public void Delete(ProdutoVaSlide produtoVaSlide)
        {
            db.ProdutoVaSlideArquivos.DeleteAllOnSubmit(produtoVaSlide.ProdutoVaSlideArquivos);
            db.ProdutoVaSlideEspecialidades.DeleteAllOnSubmit(produtoVaSlide.ProdutoVaSlideEspecialidades);
            db.ProdutoVaSlides.DeleteOnSubmit(produtoVaSlide);
        }

        public void DeleteAllEspecialidades(ProdutoVaSlide produtoVaSlide)
        {
            db.ProdutoVaSlideEspecialidades.DeleteAllOnSubmit(produtoVaSlide.ProdutoVaSlideEspecialidades);
        }

        public void AddEspecialidades(ProdutoVaSlide produtoVaSlide, int[] especialidadesIds)
        {
            EspecialidadeRepository especialidadeRepository = new EspecialidadeRepository();

            var especialidades = especialidadeRepository.GetEspecialidades().Where(e => especialidadesIds.Contains(e.Id));

            foreach (var especialidade in especialidades)
            {
                ProdutoVaSlideEspecialidade slideEspecialidade = new ProdutoVaSlideEspecialidade();
                slideEspecialidade.IdEspecialidade = especialidade.Id;
                slideEspecialidade.IdSlide = produtoVaSlide.Id;

                db.ProdutoVaSlideEspecialidades.InsertOnSubmit(slideEspecialidade);
            }
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
