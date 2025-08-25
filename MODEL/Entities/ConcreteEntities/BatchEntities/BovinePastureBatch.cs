using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class BovinePastureBatch : BatchEntity
    {
        private Guid _pastureId;
        public Guid PastureId 
        { 
            get =>_pastureId; 
            set {  _pastureId = value; }
        }
    }
}
