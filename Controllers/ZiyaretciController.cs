using Dapper;
using FuarKayitAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FuarKayitAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZiyaretciController : Controller
    {
        string connString = "DATA SOURCE = BARIS; INITIAL CATALOG = FuarKayitDB; USER ID = sa; PASSWORD = 123; Trusted_Connection = true;";
        string sorgu = "";

        [HttpGet("liste")]
        public IActionResult Listele(int id, int fuarID)
        {
            List<ZiyaretciModel> ziyaretciModel = new List<ZiyaretciModel>();

            if (id > 0)
                sorgu = "select * from tbl_ziyaretKayit where ID=" + id + " and FUAR_ID=" + fuarID;
            else if (id == 0)
                sorgu = "select * from tbl_ziyaretKayit where FUAR_ID=" + fuarID;
            else
                return BadRequest("Beklenmedik bir hata oluştu.");

            using (SqlConnection connection = new SqlConnection(connString))
            {
                ziyaretciModel = connection.Query<ZiyaretciModel>(sorgu).ToList();
            }

            return Ok(ziyaretciModel);
        }

        [HttpPost]
        public IActionResult Ekle(ZiyaretciModel ziyaretciModel)
        {
            try
            {
                ZiyaretciGecerlilikKontrol(ziyaretciModel);
                sorgu = "insert into tbl_ziyaretKayit values(@FUAR_ID, @ZIYARET_TARIH, @ZIYARETCI_AD_SOYAD, @EMAIL, @TELEFON, @FIRMA_AD, @ACIKLAMA)";
                var prms = new
                {
                    FUAR_ID = ziyaretciModel.FUAR_ID,
                    ZIYARET_TARIH = ziyaretciModel.ZIYARET_TARIH,
                    ZIYARETCI_AD_SOYAD = ziyaretciModel.ZIYARETCI_AD_SOYAD,
                    EMAIL = ziyaretciModel.EMAIL,
                    TELEFON = ziyaretciModel.TELEFON,
                    FIRMA_AD = ziyaretciModel.FIRMA_AD,
                    ACIKLAMA = ziyaretciModel.ACIKLAMA
                };


                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Execute(sorgu, prms);
                }

                return Ok();
            }
            catch (Exception ex1)
            {
                return BadRequest(ex1.Message);
            }



        }


        [HttpDelete("{id}")]
        public IActionResult Sil(int id)
        {
            if (id != null)
            {
                sorgu = "delete from tbl_ziyaretKayit where ID=" + id;

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Execute(sorgu);
                }

                return Ok();
            }
            else
                return BadRequest("Beklenmedik bir hata oluştu.");

        }

        [HttpPut]
        public IActionResult Guncelle(ZiyaretciModel ziyaretciModel)
        {
            try
            {
                ZiyaretciGecerlilikKontrol(ziyaretciModel);
                sorgu = "update tbl_ziyaretKayit set FUAR_ID=@FUAR_ID, ZIYARET_TARIH=@ZIYARET_TARIH, ZIYARETCI_AD_SOYAD=@ZIYARETCI_AD_SOYAD, EMAIL=@EMAIL," +
                "TELEFON=@TELEFON, FIRMA_AD=@FIRMA_AD, ACIKLAMA=@ACIKLAMA where ID = @ID";
                var prms = new
                {
                    ID = ziyaretciModel.ID,
                    FUAR_ID = ziyaretciModel.FUAR_ID,
                    ZIYARET_TARIH = ziyaretciModel.ZIYARET_TARIH,
                    ZIYARETCI_AD_SOYAD = ziyaretciModel.ZIYARETCI_AD_SOYAD,
                    EMAIL = ziyaretciModel.EMAIL,
                    TELEFON = ziyaretciModel.TELEFON,
                    FIRMA_AD = ziyaretciModel.FIRMA_AD,
                    ACIKLAMA = ziyaretciModel.ACIKLAMA
                };

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Execute(sorgu, prms);
                }
                return Ok();
            }
            catch (Exception Ex1)
            {
                return BadRequest(Ex1.Message);
            }
        }

        [HttpGet("ara")]
        public IActionResult Ara(string gelenAd, int fuarID)
        {
            List<ZiyaretciModel> ziyaretciModel = new List<ZiyaretciModel>();
            sorgu = "select * from tbl_ziyaretKayit where ZIYARETCI_AD_SOYAD like '%" + gelenAd + "%' and FUAR_ID=" + fuarID;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                ziyaretciModel = connection.Query<ZiyaretciModel>(sorgu).ToList();
            }

            return Ok(ziyaretciModel);
        }

        public void ZiyaretciGecerlilikKontrol(ZiyaretciModel ziyaretciModel)
        {
            string hataMesaji = "";
            if (ziyaretciModel.FUAR_ID <= 0) hataMesaji += "Fuar id'si boş olamaz! ";
            if (ziyaretciModel.ZIYARETCI_AD_SOYAD == "") hataMesaji += "Ad Soyad boş olamaz! ";
            if (ziyaretciModel.EMAIL == "") hataMesaji += "Email boş olamaz! ";
            if (ziyaretciModel.TELEFON == "") hataMesaji += "Telefon boş olamaz! ";
            if (ziyaretciModel.FIRMA_AD == "") hataMesaji += "Firma Adı boş olamaz! ";


            if (hataMesaji != "") throw new Exception("HATA OLUŞTU: " + hataMesaji);
        }
    }
}
