using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuarKayitAPI.Models
{
    public class ZiyaretciModel
    {
        public int ID { get; set; } = -1;
        public int FUAR_ID { get; set; } = -1;
        public DateTime ZIYARET_TARIH { get; set; } = DateTime.Now;
        public string ZIYARETCI_AD_SOYAD { get; set; } = "";
        public string EMAIL { get; set; } = "";
        public string TELEFON { get; set; } = "";
        public string FIRMA_AD { get; set; } = "";
        public string ACIKLAMA { get; set; } = "";
    }
}
