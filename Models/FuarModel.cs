using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuarKayitAPI.Models
{
    public class FuarModel
    {
        public int ID { get; set; } = -1;
        public int FIRMA_ID { get; set; } = -1;
        public string FUAR_AD { get; set; } = "";
        public string ACIKLAMA { get; set; } = "";
    }
}
