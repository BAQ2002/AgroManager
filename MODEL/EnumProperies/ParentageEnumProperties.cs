using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public enum BreedingType     // Tipo de cruza
    {
        Unknown = 0,               //Desconhecido   
        Natural = 1,               // monta natural
        ArtificialInsemination = 2 // sêmen utilizado Inseminação Artificial
    }

    public enum ParentFlag
    {
        Unknown = 0,               //Desconhecido
        Internal = 1,              //Pertence a fazenda 
        External = 2               //Nao pertence a fazenda
    }
}
