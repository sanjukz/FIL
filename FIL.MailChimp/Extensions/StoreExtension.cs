using System.Linq;
using System.Threading.Tasks;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;

namespace FIL.MailChimp
{
    public static class StoreExtension
    {
        public static async Task<List> GetListByName(this MailChimpManager mailChimpManager, string name)
        {
            List list = new List();
            var lists = await mailChimpManager.Lists.GetAllAsync();
            if (lists.ToList().Count > 0)
            {
                list = lists.ToList().Find(t => t.Name == name);
            }

            return list;
        }

        public static  async Task<Store> GetOrCreateStore(this MailChimpManager mailChimpManager, List list)
        {
            var store = new Store();
            try
            {
                store = await mailChimpManager.ECommerceStores.GetAsync("fil");
            }
            catch (System.Exception)
            {
                store = await mailChimpManager.ECommerceStores.AddAsync(new Store
                {
                    Id = "fil-01",
                    ListId = list.Id,
                    Name = "Feel It Live",
                    EmailAddress = "vivek.singh@gmail.com",
                    CurrencyCode = CurrencyCode.USD,
                    PrimaryLocale = "en",
                });
            }
            return store;
        }

        public static async Task<Customer> GetCustomerByEmail(this MailChimpManager mailChimpManager, string storeId, string email)
        {
            Customer customer = new Customer();
            var customers = await mailChimpManager.ECommerceStores.Customers(storeId).GetAllAsync();
            if (customers.ToList().Count > 0)
            {
                customer = customers.ToList().Find(t => t.EmailAddress == email);
                return customer;
            }
            return customer;
        }
    }
}