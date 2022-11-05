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
    public class FuarController : Controller
    {
        string connString = "DATA SOURCE = BARIS; INITIAL CATALOG = FuarKayitDB; USER ID = sa; PASSWORD = 123; Trusted_Connection = true;";
        string sorgu = "";

        [HttpGet("liste")]
        public IActionResult Listele(int id, int firmaID)
        {
            List<FuarModel> fuarModel = new List<FuarModel>();

            if (id > 0)
                sorgu = "select * from tbl_fuarKayit where ID=" + id + " and FIRMA_ID=" + firmaID;
            else if (id == 0)
                sorgu = "select * from tbl_fuarKayit where FIRMA_ID=" + firmaID;
            else
                return BadRequest("Beklenmedik bir hata oluştu.");


            using (SqlConnection connection = new SqlConnection(connString))
            {
                fuarModel = connection.Query<FuarModel>(sorgu).ToList();
            }

            return Ok(fuarModel);
        }

        [HttpPost]
        public IActionResult Ekle(FuarModel fuarModel)
        {
            try
            {
                FuarGecerlilikKontrol(fuarModel);
                sorgu = "insert into tbl_fuarKayit values(@FIRMA_ID, @FUAR_AD, @ACIKLAMA)";
                var prms = new
                {
                    FIRMA_ID = fuarModel.FIRMA_ID,
                    FUAR_AD = fuarModel.FUAR_AD,
                    ACIKLAMA = fuarModel.ACIKLAMA
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
            try
            {
                sorgu = "delete from tbl_fuarKayit where ID=" + id;

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Execute(sorgu);
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Beklenmedik bir hata oluştu.");
            }
        }

        [HttpPut]
        public IActionResult Guncelle(FuarModel fuarModel)
        {
            try
            {
                FuarGecerlilikKontrol(fuarModel);
                sorgu = "update tbl_fuarKayit set FIRMA_ID = @FIRMA_ID, FUAR_AD = @FUAR_AD, ACIKLAMA = @ACIKLAMA where ID = @ID";
                var prms = new
                {
                    ID = fuarModel.ID,
                    FIRMA_ID = fuarModel.FIRMA_ID,
                    FUAR_AD = fuarModel.FUAR_AD,
                    ACIKLAMA = fuarModel.ACIKLAMA
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


        [HttpGet("ara")]
        public IActionResult Ara(string gelenAd, int firmaID)
        {
            List<FuarModel> fuarModel = new List<FuarModel>();
            sorgu = "select * from tbl_fuarKayit where FUAR_AD like '%" + gelenAd + "%' and FIRMA_ID=" + firmaID;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                fuarModel = connection.Query<FuarModel>(sorgu).ToList();
            }

            return Ok(fuarModel);
        }


        public void FuarGecerlilikKontrol(FuarModel fuarModel)
        {
            string hataMesaji = "";
            if (fuarModel.FUAR_AD == "") hataMesaji += "Fuar Adı boş olamaz! ";


            if (hataMesaji != "") throw new Exception("HATA OLUŞTU: " + hataMesaji);
        }
    }
}
