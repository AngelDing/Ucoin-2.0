using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Ucoin.Framework.Utility;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.Entities;
using System.Diagnostics;

namespace Ucoin.Framework.Test
{
    public class EFRepositoryTests : DisposableObject
    {
        //private string dbName = ConstHelper.EFTestDBName;

        public EFRepositoryTests()
        {
            InitializeEFTestDB();
        }

        private void InitializeEFTestDB()
        {
            Database.SetInitializer<EFTestContext>(new DropCreateDatabaseIfModelChanges<EFTestContext>());
            using (var context = new EFTestContext())
            {
                context.Database.Initialize(true);
            }
            using (var context = new EFTestContext())
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
            using (var context = new EFTestContext())
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
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);

            var custList = new List<EFCustomer>();           
            using (var newRepo = new CustomerRepository())
            {
                custList = newRepo.GetAll().ToList();
            }
            custList.Count.Should().Be(1);
            custList.First().UserName.Should().Be("daxnet");
            custList.First().Id.Should().BeGreaterThan(0);

            using (var newRepo = new CustomerRepository())
            {
                var newCustomer = newRepo.GetCustomerByName("daxnet");
                var list = newRepo.GetNoteList();
            }
        }

        private void InsertNewCustomer(EFCustomer customer)
        {
            using (var custRepo = new CustomerRepository())
            {
                var actual = custRepo.GetAll().ToList().Count;
                //actual.Should().Be(0);

                custRepo.Insert(customer);
                custRepo.RepoContext.Commit();
            }
        }

        private EFCustomer GetCustomerInfo()
        {
            var customer = new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "daxnet",
                Phone = "111111",
                Password = "123456",
                EFNote = new List<EFNote> { new EFNote { NoteText = "AA" }, new EFNote { NoteText = "BB" } }
            };
            return customer;
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
            var cId = 0;
            bool isExists;
            using (var repo = new CustomerRepository())
            {
                repo.Insert(customers);
                repo.RepoContext.Commit();

                getByList = repo.GetBy(p => p.UserName.StartsWith("d") && p.Password != "dd").ToList();
                allList = repo.GetAll().ToList();
                cId = allList.Find(p => p.UserName == "dd").Id;

                foreach (var c in allList)
                {
                    //c.UserName = c.UserName + "123";
                    c.SetUpdate(() => c.UserName, c.UserName + "123");
                }
                repo.Update(allList);
                repo.RepoContext.Commit();

                updateList = repo.GetAll().ToList();

                repo.Delete(customers[0]);
                repo.RepoContext.Commit();
                repo.Delete(cId);
                repo.RepoContext.Commit();
                repo.Delete(p => p.Password == "bb");
                repo.RepoContext.Commit();
                deleteList = repo.GetAll().ToList();
                isExists = repo.Exists(p => p.Password == "cc");
            }

            getByList.Count().Should().Be(1);
            allList.Count.Should().Be(5);
            foreach (var c in allList)
            {
                c.Id.Should().BeGreaterThan(0);
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
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);

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
            custList.First().Id.Should().BeGreaterThan(0);
        }

        #region Update
        [Fact]
        public void ef_update_by_auto_compare_by_ilist_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
                cInfo.EFNote = cInfo.EFNote.ToList();
            }

            var updateInfo = cInfo.DeepCopy();
            updateInfo.Email = "jacky@ucoin.com";
            updateInfo.Phone = null;
            updateInfo.Address.City = "SZ";
            updateInfo.EFNote.First().NoteText = "DDDD";
            var newNotes = updateInfo.EFNote.ToList();
            newNotes.RemoveAll(p => p.Id == 2);
            updateInfo.EFNote = newNotes;
            updateInfo.EFNote.Add(new EFNote { NoteText = "CCCC" });

            var result = new CompareLogic().Compare(updateInfo, cInfo);

            using (var repo = new CustomerRepository())
            {
                repo.Update(updateInfo);
                repo.RepoContext.Commit();
            }
            using (var repo = new CustomerRepository())
            {
                var newInfo = repo.GetCustomFullInfo(1);
                newInfo.Email.Should().Be("jacky@ucoin.com");
                newInfo.EFNote.Count().Should().Be(2);
            }
        }

        [Fact]
        public void ef_update_by_auto_compare_by_hashset_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            var updateInfo = cInfo.DeepCopy();
            updateInfo.Email = "jacky@ucoin.com";
            updateInfo.Phone = null;
            updateInfo.Address.City = "SZ";
            updateInfo.EFNote.First().NoteText = "DDDD";
            var newNotes = updateInfo.EFNote;
            var note = newNotes.Last();
            newNotes.Remove(note);
            updateInfo.EFNote = newNotes;
            updateInfo.EFNote.Add(new EFNote { NoteText = "CCCC" });

            var result = new CompareLogic().Compare(updateInfo, cInfo);

            using (var repo = new CustomerRepository())
            {
                repo.Update(updateInfo);
                repo.RepoContext.Commit();
            }
            using (var repo = new CustomerRepository())
            {
                var newInfo = repo.GetCustomFullInfo(1);
                newInfo.Email.Should().Be("jacky@ucoin.com");
                newInfo.EFNote.Count().Should().Be(2);
            }
        }        

        [Fact]
        public void ef_update_by_manual_compare_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.Email = "jacky@ucoin.com";
            cInfo.Address.City = "SZ";
            cInfo.Address.Zip = "000000000";
            cInfo.ObjectState = ObjectStateType.Modified;
            cInfo.EFNote.First().NoteText = "DDDD";
            cInfo.EFNote.First().ObjectState = ObjectStateType.Modified;
            cInfo.EFNote.Last().ObjectState = ObjectStateType.Deleted;
            cInfo.EFNote.Add(new EFNote { NoteText = "CCCC", ObjectState = ObjectStateType.Added });

            using (var repo = new CustomerRepository())
            {
                repo.Update(cInfo);
                repo.RepoContext.Commit();
            }

            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }
            cInfo.Email.Should().Be("jacky@ucoin.com");
            var address = cInfo.Address;
            address.City.Should().Be("SZ");
            address.Zip.Should().Be("000000000");
            cInfo.EFNote.Count.Should().Be(2);
            cInfo.EFNote.Last().NoteText.Should().Be("CCCC");
            cInfo.EFNote.First().NoteText.Should().Be("DDDD");
        }

        [Fact]
        public void ef_update_by_partial_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.SetUpdate(() => cInfo.Address.City, "SZ");
            cInfo.SetUpdate(() => cInfo.Address.Zip, "000000000");
            cInfo.SetUpdate(() => cInfo.Email, "jacky@ucoin.com");
            
            var firstNote = cInfo.EFNote.First();
            firstNote.SetUpdate(() => firstNote.NoteText, "DDDD");
            
            using (var repo = new CustomerRepository())
            {
                repo.Update(cInfo);
                repo.RepoContext.Commit();
            }

            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }
            cInfo.Email.Should().Be("jacky@ucoin.com");
            var address = cInfo.Address;
            address.City.Should().Be("SZ");
            address.Zip.Should().Be("000000000");
            cInfo.EFNote.Count.Should().Be(2);
            cInfo.EFNote.Last().NoteText.Should().Be("BB");
            cInfo.EFNote.First().NoteText.Should().Be("DDDD");
        }

        [Fact]
        public void ef_update_by_auto_complex_compare_test()
        {
            var customer = GetComplexCustomerInfo();
            InsertNewCustomer(customer);

            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            var updateInfo = cInfo.DeepCopy();
            updateInfo.Email = "jacky@ucoin.com";
            updateInfo.Phone = null;
            updateInfo.Address.City = "SZ";
            updateInfo.EFNote.First().NoteText = "DDDD";
            var newNotes = updateInfo.EFNote;
            var note = newNotes.Last();
            newNotes.Remove(note);
            updateInfo.EFNote = newNotes;
            var childList = updateInfo.EFNote.First().ChildNote;
            var cNote = childList.First();
            childList.Remove(cNote);
            childList.First().Title = "MMM";
            childList.Add(new ChildNote { Title = "ZZZ" });          
            updateInfo.EFNote.Add(new EFNote { NoteText = "CCCC" });

            var result = new CompareLogic().Compare(updateInfo, cInfo);

            using (var repo = new CustomerRepository())
            {
                repo.Update(updateInfo);
                repo.RepoContext.Commit();
            }
            using (var repo = new CustomerRepository())
            {
                var newInfo = repo.GetCustomFullInfo(1);
                newInfo.Email.Should().Be("jacky@ucoin.com");
                newInfo.EFNote.Count().Should().Be(2);
                var newChilds = newInfo.EFNote.First().ChildNote;
                newChilds.Count.Should().Be(2);
                newChilds.First().Title.Should().Be("MMM");
                newChilds.Last().Title.Should().Be("ZZZ");
            }
        }

        private EFCustomer GetComplexCustomerInfo()
        {
            var customer = new EFCustomer
            {
                Address = new EFAddress("China", "SH", "SH", "A street", "12345"),
                UserName = "daxnet",
                Phone = "111111",
                Password = "123456",
                EFNote = new List<EFNote> 
                { 
                    new EFNote { NoteText = "AA" }, 
                    new EFNote
                    { 
                        NoteText = "BB",
                        ChildNote = new List<ChildNote>
                        {
                            new ChildNote{Title = "XXX"}, 
                            new ChildNote{Title = "YYY"}
                        }
                    } 
                }
            };
            return customer;
        }
        #endregion

        #region Delete

        [Fact]
        public void ef_delete_by_key_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            using (var repo = new CustomerRepository())
            {
                repo.Delete(1);
                repo.RepoContext.Commit();
                var notes = repo.GetNoteList();
                var cInfo = repo.GetByKey(1);
                cInfo.Should().BeNull();
                notes.Count.Should().Be(0);
            }

            using (var repo = new CustomerRepository())
            {
                var notes = repo.GetNoteList();
                var cInfo = repo.GetByKey(1);
                cInfo.Should().BeNull();
                notes.Count.Should().Be(0);
            }
        }

        [Fact]
        public void ef_delete_by_entity_test()
        {
            var customer = GetCustomerInfo();
            InsertNewCustomer(customer);
            using (var repo = new CustomerRepository())
            {
                var cInfo = repo.GetByKey(1);
                repo.Delete(cInfo);
                repo.RepoContext.Commit();
            }
            using (var repo = new CustomerRepository())
            {
                var cInfo = repo.GetByKey(1);
                var notes = repo.GetNoteList();
                cInfo.Should().BeNull();
                notes.Count.Should().Be(0);
            }
        }

        #endregion
    }
}
