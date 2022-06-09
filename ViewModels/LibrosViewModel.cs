using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class LibrosViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(15)]
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Temas { get; set; }
        public string Editorial { get; set; }
    }
}
