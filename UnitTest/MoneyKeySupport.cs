using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using wg_core.Domain;
using wg_utils;
using System.IO;

namespace UnitTest
{
    public class MoneyKeySupport
    {
        public List<string> AddMoneyKey(string plat, int money, int count)
        {
            List<string> mk = new List<string>();
            for (int i = 0; i < count; i++)
            {
                mk.Add(SystemUtil.GenerateStringId() + "-" + money.ToString());
            }
            //生成文件
            var path = AppDomain.CurrentDomain.BaseDirectory + "/" + $"{DateTime.Now.ToString("yyyyMMddHHmmss")}-{plat}-{money}-{count}.txt";
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            mk.ForEach(item=> {
                sw.WriteLine(item + ",");
            });
            sw.Flush();
            sw.Close();
            fs.Close();


            using (var db = new ShopContext())
            {
                var entitys = mk.Select(item => new t1_user_moneykey
                {
                    mony_key = item,
                    price = money,
                    used = "N",
                    created_time = DateTime.Now,
                    plat = plat
                }).ToList();
                db.t1_user_moneykey.AddRange(entitys);
                db.SaveChanges();
            }
            return new List<string>();
        }
    }
}
