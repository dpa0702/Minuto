using System;
using System.Collections.Generic;

namespace Demetrios.Models
{
    public class MinutoPost
    {
        public string id { get; set; }

        public string title { get; set; }

        public string link { get; set; }

        public string description { get; set; }

        public int quantidade { get; set; }

        public List<PrincipaisPalavras> pPalavras { get; set; }

        public DateTime DataAlteracao { get; set; }
    }

    public class PrincipaisPalavras
    {
        public int Id { get; set; }
        public string descricao { get; set; }
        public int ocorrencias { get; set; }
    }
}
