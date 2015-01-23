using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace Ucoin.Framework.Test
{
    public class EFRepositoryTests : DisposableObject
    {
        private string dbName = ConstHelper.EFTestDBName;

        public EFRepositoryTests()
        {
            InitializeEFTestDB();
        }

        private void InitializeEFTestDB()
        {
            Database.SetInitializer<EFTestContext>(new DropCreateDatabaseIfModelChanges<EFTestContext>());
            using (var context = new EFTestContext(dbName))
            {
                context.Database.Initialize(true);
            }
            using (var context = new EFTestContext(dbName))
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }

                context.Database.Create();
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (var context = new EFTestContext(dbName))
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
            }
        }

        [Fact]
        public void ef_save_aggregate_root_test()
        {
            InsertNewCustomer();

            var custList = new List<EFCustomer>();           
            using (var newRepo = new CustomerRepository())
            {
                custList = newRepo.GetAll().ToList();
                custList.First().Notes.First().NoteText.Should().Be("AA");    //延遲加載
            }
            custList.Count.Should().Be(1);
            custList.First().UserName.Should().Be("daxnet");
            custList.First().Id.Should().NotBe(Guid.Empty);

            using (var newRepo = new CustomerRepository())
            {
                var newCustomer = newRepo.GetCustomerByName("daxnet");
                var list = newRepo.GetNoteList();
            }
        }

        private static void InsertNewCustomer()
        {
            var customer = new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "daxnet",
                Phone = "111111",
                Password = "123456",
                Notes = new List<EFNote> { new EFNote { NoteText = "AA" }, new EFNote { NoteText = "BB" } }
            };

            using (var custRepo = new CustomerRepository())
            {
                var actual = custRepo.GetAll().ToList().Count;
                actual.Should().Be(0);

                custRepo.Insert(customer);
                custRepo.Context.Commit();
            }
        }

        [Fact]
        public void ef_batch_test()
        {
            List<EFCustomer> customers = new List<EFCustomer>{new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "daxnet",
                Password = "123456"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "aa",
                Password = "aa"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "bb",
                Password = "bb"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "cc",
                Password = "cc"
            },
            new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "dd",
                Password = "dd"
            }};

            var getByList = new List<EFCustomer>();
            var allList = new List<EFCustomer>();
            var updateList = new List<EFCustomer>();
            var deleteList = new List<EFCustomer>();
            var guid = Guid.Empty;
            bool isExists;
            using (var repo = new CustomerRepository())
            {
                repo.Insert(customers);
                repo.Context.Commit();

                getByList = repo.GetBy(p => p.UserName.StartsWith("d") && p.Password != "dd").ToList();
                allList = repo.GetAll().ToList();
                guid = allList.Find(p => p.UserName == "dd").Id;

                foreach (var c in allList)
                {
                    c.UserName = c.UserName + "123";
                }
                repo.Update(allList);
                repo.Context.Commit();

                updateList = repo.GetAll().ToList();

                repo.Delete(customers[0]);
                repo.Context.Commit();
                repo.Delete(guid);
                repo.Context.Commit();
                repo.Delete(p => p.Password == "bb");
                repo.Context.Commit();
                deleteList = repo.GetAll().ToList();
                isExists = repo.Exists(p => p.Password == "cc");
            }

            getByList.Count().Should().Be(1);
            allList.Count.Should().Be(5);
            foreach (var c in allList)
            {
                c.Id.Should().NotBe(Guid.Empty);
            }
            foreach (var c in updateList)
            {
                c.UserName.Should().Contain("123");
            }
            deleteList.Count().Should().Be(2);
            isExists.Should().Be(true);
        }

        [Fact]
        public void ef_spec_test()
        {
            InsertNewCustomer();

            var custList = new List<EFCustomer>();
            var cQuery = new CustomerQueryCriteria
            {
                UserName = "daxnet"
            };
            var spec = CustomerSpecification.GetCustomerByFilter(cQuery);

            using (var repo = new CustomerRepository())
            {              
                custList = repo.GetBy(spec).ToList();               
            }

            custList.Count.Should().Be(1);
            custList.First().UserName.Should().Be("daxnet");
            custList.First().Id.Should().NotBe(Guid.Empty);
        }
    }
}
