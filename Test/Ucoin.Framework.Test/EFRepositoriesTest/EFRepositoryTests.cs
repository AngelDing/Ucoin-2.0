﻿using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using Ucoin.Utility;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.Entities;

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
            InsertNewCustomer();

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

        private void InsertNewCustomer()
        {
            var customer = GetCustomerInfo();

            using (var custRepo = new CustomerRepository())
            {
                var actual = custRepo.GetAll().ToList().Count;
                actual.Should().Be(0);

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
                Notes = new List<EFNote> { new EFNote { NoteText = "AA" }, new EFNote { NoteText = "BB" } }
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
            custList.First().Id.Should().BeGreaterThan(0);
        }

        #region Update
        [Fact]
        public void ef_update_by_auto_compare_test()
        {
            InsertNewCustomer();
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            var updateInfo = cInfo.DeepCopy();
            updateInfo.Email = "jacky@ucoin.com";
            updateInfo.Phone = null;
            updateInfo.Address.City = "SZ";
            updateInfo.Notes.First().NoteText = "DDDD";
            var newNotes = updateInfo.Notes.ToList();
            newNotes.RemoveAll(p => p.Id == 2);
            updateInfo.Notes = newNotes;
            updateInfo.Notes.Add(new EFNote { NoteText = "CCCC" });

            var result = new CompareLogic().Compare(updateInfo, cInfo);

            using (var repo = new CustomerRepository())
            {
                repo.FullUpdate(updateInfo, result);
                repo.RepoContext.Commit();
            }
            using (var repo = new CustomerRepository())
            {
                var newInfo = repo.GetCustomFullInfo(1);
                newInfo.Email.Should().Be("jacky@ucoin.com");
                newInfo.Notes.Count().Should().Be(2);
            }
        }

        [Fact]
        public void ef_update_by_auto_compare_by_list_test()
        {
            //InsertNewCustomer();
            //InsertNewCustomer();

            //var cInfo = new EFCustomer();
            //using (var repo = new CustomerRepository())
            //{
            //    cInfo = repo.GetCustomFullInfo(1);
            //}

            //var updateInfo = cInfo.DeepCopy();
            //updateInfo.Email = "jacky@ucoin.com";
            //updateInfo.Phone = null;
            //updateInfo.Address.City = "SZ";
            //updateInfo.Notes.First().NoteText = "DDDD";
            //var newNotes = updateInfo.Notes.ToList();
            //newNotes.RemoveAll(p => p.Id == 2);
            //updateInfo.Notes = newNotes;
            //updateInfo.Notes.Add(new EFNote { NoteText = "CCCC" });

            //var result = new CompareLogic().Compare(updateInfo, cInfo);

            //using (var repo = new CustomerRepository())
            //{
            //    repo.FullUpdate(updateInfo, result);
            //    repo.RepoContext.Commit();
            //}
            //using (var repo = new CustomerRepository())
            //{
            //    var newInfo = repo.GetCustomFullInfo(1);
            //    newInfo.Email.Should().Be("jacky@ucoin.com");
            //    newInfo.Notes.Count().Should().Be(2);
            //}
        }


        [Fact]
        public void ef_update_by_manual_compare_test()
        {
            InsertNewCustomer();
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.Email = "jacky@ucoin.com";
            cInfo.Address.City = "SZ";
            cInfo.Address.Zip = "000000000";
            cInfo.State = ObjectStateType.Modified;
            cInfo.Notes.First().NoteText = "DDDD";
            cInfo.Notes.First().State = ObjectStateType.Modified;
            cInfo.Notes.Last().State = ObjectStateType.Deleted;
            cInfo.Notes.Add(new EFNote { NoteText = "CCCC", State = ObjectStateType.Added });

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
            cInfo.Notes.Count.Should().Be(2);
            cInfo.Notes.Last().NoteText.Should().Be("CCCC");
            cInfo.Notes.First().NoteText.Should().Be("DDDD");
        }

        [Fact]
        public void ef_update_by_partial_test()
        {
            InsertNewCustomer();
            var cInfo = new EFCustomer();
            using (var repo = new CustomerRepository())
            {
                cInfo = repo.GetCustomFullInfo(1);
            }

            cInfo.SetUpdate(() => cInfo.Address.City, "SZ");
            cInfo.SetUpdate(() => cInfo.Address.Zip, "000000000");
            cInfo.SetUpdate(() => cInfo.Email, "jacky@ucoin.com");
            
            var firstNote = cInfo.Notes.First();
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
            cInfo.Notes.Count.Should().Be(2);
            cInfo.Notes.Last().NoteText.Should().Be("BB");
            cInfo.Notes.First().NoteText.Should().Be("DDDD");
        }
        #endregion

        #region Delete

        [Fact]
        public void ef_delete_by_key_test()
        {
            InsertNewCustomer();
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
            InsertNewCustomer();
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
