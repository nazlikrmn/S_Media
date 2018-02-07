using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webApp.Models;

namespace webApp.Controllers
{
    public class ValuesController : ApiController
    {
        public bool KullaniciGirisi(string Username, string pass)//kullanıcı adı şifresi kontrol. doğruysa true
        {
            using (var context = new instEntities1())
            {
                var sorgu = from p in context.kullaniciLogin
                            where p.pass == pass && p.username == Username
                            select p;
                if (sorgu.Any())
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool KullaniciGirisiWithMail(string Mail, string pass)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from b in context.kullaniciBilgi
                            join l in context.kullaniciLogin
                            on b.kullaniciId equals l.kulBilgiId
                            where b.mail == Mail && l.pass == pass
                            select b;
                if (sorgu.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
       
        public List<string> GetTakipciList(int kullaniciLoginId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from t in context.Takip where t.TakipEdilen == getKullaniciBilgiId(kullaniciLoginId) select t;
                return getUsernames(sorgu);
            }
        }//takipçiusernames
        public List<string> GetTakipEdilenlerList(int kullaniciLoginId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from t in context.Takip where t.TakipEden == getKullaniciBilgiId(kullaniciLoginId) select t;
                return getUsernames(sorgu);
            }
        }//takip edilenler usernames
        private List<string> getUsernames(IQueryable<Takip> queryable)
        {
            List<string> list = new List<string>();
            using (var context = new instEntities1())
            {
                foreach (var item in queryable)
                {
                    var username = from b in context.kullaniciLogin where item.TakipEdilen == b.kulBilgiId select b.username;
                    list.Add(username.ToString());
                }
                return list;
            }
        }
        public List<string> KullanicininProfili(int kullaniciLoginId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from a in context.Gonderi where a.loginId == kullaniciLoginId select a.Url;
                return sorgu.ToList();
            }
        }//kullanıcının gönderi url leri
        
        private int getKullaniciBilgiId(int kullaniciLoginId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from l in context.kullaniciLogin where l.loginId == kullaniciLoginId select l.kulBilgiId;
                return Convert.ToInt32(sorgu);
            }
        }
        public List<string> getBegenenler(int gonderiId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from b in context.Begeni where b.GonderiId == gonderiId select b.BegenenKisiId;
                List<string> usernames = new List<string>();
                foreach (var kisiID in sorgu)
                {
                    var username = from l in context.kullaniciLogin where l.loginId == kisiID select l.username;
                    usernames.Add(username.ToString());
                }
                return usernames;
            }
        }//gönderiyi begenen kişiler-usernames
        public IQueryable<Yorum> getYorumlar(int gonderiId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from y in context.Yorum where y.gonderiId == gonderiId select y;
                return sorgu.OrderBy(x => new { x.tarih });
            }
        }//gönderinin yorumları
        public int getYorumSayisi(int gonderiId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from a in context.Yorum where a.gonderiId == gonderiId select a;
                return sorgu.Count();
            }
        }//yorum sayısı
        public int getBegeniSayisi(int gonderiId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from a in context.Begeni where a.GonderiId == gonderiId select a;
                return sorgu.Count();
            }
        }//begeni sayisi
        public IQueryable<Gonderi> getDashBoard(int kullaniciBilgiId)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from g in context.Gonderi where takipEdiyorMu(kullaniciBilgiId, g.loginId) select g;
                return sorgu.OrderBy(x => x.tarih);
            }
        }//ana ekranı getir
        public bool takipEdiyorMu(int girisYapan, int digerKullanici)
        {
            using (var context = new instEntities1())
            {
                var sorgu = from t in context.Takip where t.TakipEden == girisYapan && t.TakipEdilen == getKullaniciBilgiId(digerKullanici) select t;
                if (sorgu.Any())
                    return true;
                else return false;
            }
        }// takip kontrolü
        public int getLoginId(string bilgi)
        {
            using (var context=new instEntities1())
            {
                if (bilgi.IndexOf('@') > -1)
                {
                    var sorgu = from b in context.kullaniciBilgi
                                join l in context.kullaniciLogin
                                on b.kullaniciId equals l.kulBilgiId
                                where b.mail == bilgi
                                select l.loginId;
                    return Convert.ToInt32(sorgu);
                }
                else
                {
                    var sorgu = from l in context.kullaniciLogin where bilgi == l.username select l.loginId;
                    return Convert.ToInt32(sorgu);
                }
            }
            return -1;
        }

    }

}

