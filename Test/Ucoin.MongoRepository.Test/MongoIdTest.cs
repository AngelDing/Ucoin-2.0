using System.Linq;
using Xunit;
using FluentAssertions;
using MongoDB.Bson.Serialization;
using Ucoin.Framework.MongoRepository;
using Ucoin.Framework.MongoRepository.IdGenerators;
using System.Threading.Tasks;
using System;

namespace Ucoin.MongoRepository.Test
{
    public class MongoIdTest : BaseMongoTest
    {
        [Fact]
        public void mongo_string_id_test()
        {
            var pInfo = new Product
            {
               Description = "XXX",
               Name = "aa",
               Price = 100               
            };

            var repo = new MongoTestDB<Product>();
            repo.Insert(pInfo);

            var added = repo.GetBy(p => p.Name == "aa").FirstOrDefault();
            added.Should().NotBeNull();
            added.Id.Should().NotBeEmpty();
            added.Price.Should().Be(100);
        }

        [Fact]
        public void mongo_long_id_generator_test()
        {
            var log = new OrderLog
            {
                Summary = "test",
                Title = "aa"
            };

            var repo = new MongoTestDB<OrderLog, long>();
            repo.Insert(log);

            var added = repo.GetBy(p => p.Title == "aa").FirstOrDefault();
            added.Should().NotBeNull();
            added.Id.Should().Be(1);
            added.Summary.Should().Be("test");
        }

        [Fact]
        public void mongo_long_id_generator_concurrency_test()
        {
            var repo = new MongoTestDB<CustomEntityTest, int>();

            var count = 1000;
            var tasks = new Task[count];
            for (var i = 0; i < count; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var custom = new CustomEntityTest()
                    {
                        Name = "jia" + DateTime.Now.ToLongDateString()
                    };

                    repo.Insert(custom);
                });
            }
            Task.WaitAll(tasks);

            var list = repo.GetAll().ToList();
            list.Count.Should().Be(count);
            var maxId = list.Max(p => p.Id);
            maxId.Should().Be(count);
        }
    }
}
