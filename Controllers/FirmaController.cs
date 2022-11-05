using FuarKayitAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace FuarKayitAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FirmaController : Controller
    {
        string connString = "DATA SOURCE = BARIS; INITIAL CATALOG = FuarKayitDB; USER ID = sa; PASSWORD = 123; Trusted_Connection = true;";
        string sorgu = "";

        [HttpGet("liste")]
        public IActionResult Listele(int id)
        {
            List<FirmaModel> firmaModel = new List<FirmaModel>();

            if (id > 0)
                sorgu = "select * from tbl_firmaKayit where ID=" + id;
            else
                sorgu = "select * from tbl_firmaKayit";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                firmaModel = connection.Query<FirmaModel>(sorgu).ToList();
            }

            return Ok(firmaModel);
        }

        [HttpPost]
        public IActionResult Ekle(FirmaModel firmaModel)
        {
            sorgu = "insert into tbl_firmaKayit values(@FIRMA_AD, @EMAIL, @SIFRE)";
            var prms = new
            {
                FIRMA_AD = firmaModel.FIRMA_AD,
                EMAIL = firmaModel.EMAIL,
                SIFRE = firmaModel.SIFRE
            };


            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Execute(sorgu, prms);
            }

            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult Sil(int id)
        {
            sorgu = "delete from tbl_firmaKayit where ID=" + id;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Execute(sorgu);
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Guncelle(FirmaModel firmaModel)
        {
            sorgu = "update tbl_firmaKayit set FIRMA_AD = @FIRMA_AD, EMAIL = @EMAIL, SIFRE = @SIFRE where ID = @ID";
            var prms = new
            {
                ID = firmaModel.ID,
                FIRMA_AD = firmaModel.FIRMA_AD,
                EMAIL = firmaModel.EMAIL,
                SIFRE = firmaModel.SIFRE
            };

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Execute(sorgu, prms);
            }

            return Ok();
        }

        [HttpGet("ara")]
        public IActionResult Ara(string gelenAd)
        {
            List<FirmaModel> firmaModel = new List<FirmaModel>();
            sorgu = "select * from tbl_firmaKayit where FIRMA_AD like '%" + gelenAd + "%'";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                firmaModel = connection.Query<FirmaModel>(sorgu).ToList();
            }

            return Ok(firmaModel);
        }

        public FirmaModel fLoginKontrol(FirmaModel aModel)
        {
            var connection = new SqlConnection(connString);
            sorgu = "select * from tbl_firmaKayit where EMAIL = @email and SIFRE = @sifre";
            var AFirma = connection.Query<FirmaModel>(sorgu, new { email = aModel.EMAIL, sifre = aModel.SIFRE }).FirstOrDefault();
            if (AFirma == null)
            {
                throw new Exception("Kullanıcı adı veya şifre hatalı!");
            }
            else
            {
                AFirma.SIFRE = "";
                return AFirma;
            }
        }

        [HttpPost("loginKontrol")]
        public IActionResult LoginKontrol(FirmaModel aModel)
        {
            try
            {
                return Ok(fLoginKontrol(aModel));
            }
            catch (Exception Ex1)
            {
                return BadRequest(Ex1.Message);
            }
        }

    }
}
