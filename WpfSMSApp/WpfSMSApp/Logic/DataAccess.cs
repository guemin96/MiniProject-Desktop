using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSMSApp.Model;

namespace WpfSMSApp.Logic
{
    public class DataAccess
    {
        public static List<User> GetUsers()
        {
            List<User> users;

            using (var ctx = new SMSEntities())
            {
                users = ctx.User.ToList();
            }
            return users;
        }
        /// <summary>
        /// 입력, 수정 동시에....
        /// </summary>
        /// <returns>0또는 1이상</returns>
        public static int SetUser(User user)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.User.AddOrUpdate(user); // 인덱스값이 없는 경우에는 add 인덱스값이 있는 경우에는 update
                return ctx.SaveChanges();// savechange?는 무슨 함수?
            }
           
        }

        public static List<Stock> GetStocks()
        {
            List<Stock> stocks;

            using (var ctx = new SMSEntities())//ctx = datacontext
            {
                stocks = ctx.Stock.ToList();
            }
            return stocks;
        }

        public static List<Store> GetStores()
        {
            List<Store> stores;

            using (var ctx = new SMSEntities())//ctx = datacontext
            {
                stores = ctx.Store.ToList();
            }
            return stores;
        }
        public static int SetStores(Store store)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Store.AddOrUpdate(store); // 인덱스값이 없는 경우에는 add 인덱스값이 있는 경우에는 update
                return ctx.SaveChanges();// savechange?는 무슨 함수?
            }

        }
    }
}
