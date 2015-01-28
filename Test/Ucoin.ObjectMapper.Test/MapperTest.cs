﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Ucoin.Framework.ObjectMapper.Test
{
    public class MapperTest
    {
        [Fact]
        public void TestMapCollection()
        {
            var roleEntities = new RoleEntity[10];
            for (int i = 0; i < roleEntities.Length; i++)
            {
                roleEntities[i] = new RoleEntity
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = "Manager" + i,
                    LoweredRoleName = "manager" + i
                };
            }
            var roles = new Role[roleEntities.Length];
            for (int i = 0; i < roleEntities.Length; i++)
            {
                roles[i] = new Role
                {
                    RoleId = roleEntities[i].RoleId,
                    RoleName = roleEntities[i].RoleName,
                    Description = roleEntities[i].Description
                };
            }
            // Array
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity, Role>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], Role[]>(roleEntities));
            // IEnumerable
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity, Role>((IEnumerable<RoleEntity>)roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], IEnumerable<Role>>(roleEntities));
            // ICollection
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity, Role>((ICollection<RoleEntity>)roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ICollection<Role>>(roleEntities));
            // IList
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity, Role>((IList<RoleEntity>)roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], IList<Role>>(roleEntities));
            // List
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity, Role>(new List<RoleEntity>(roleEntities)));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], List<Role>>(roleEntities));
            // Custom Collection
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ReadOnlyRoleCollection>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ReadOnlyCollection<Role>>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ReadOnlyRoleCollection1>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ReadOnlyRoleCollection2>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ReadOnlyRoleCollection3>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], ReadOnlyRoleCollection4>(roleEntities));
            Helper.AreSequentialEqual(roles, Mapper.Map<RoleEntity[], RoleCollection>(roleEntities));
        }

        [Fact]
        public void TestMapInstance()
        {
            var entity = new RoleEntity
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Manager",
                LoweredRoleName = "manager",
                Description = "Department or group manager"
            };
            var role = Mapper.Map<RoleEntity, Role>(entity);
            Assert.Equal(entity.RoleId, role.RoleId);
            Assert.Equal(entity.RoleName, role.RoleName);
            Assert.Equal(entity.Description, role.Description);
        }

        [Fact]
        public void TestCustomMemberMapper()
        {
            var container = new ObjectMapper();
            container.Configure<Role, RoleEntity>()
                .MapMember(target => target.LoweredRoleName, source => source.RoleName.ToLower());
            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Manager",
                Description = "Department or group manager"
            };
            var entity = container.Map<Role, RoleEntity>(role);
            Assert.Equal(entity.RoleId, role.RoleId);
            Assert.Equal(entity.RoleName, role.RoleName);
            Assert.Equal(entity.LoweredRoleName, role.RoleName.ToLower());
            Assert.Equal(entity.Description, role.Description);
        }

        [Fact]
        public void TestCustomCreator()
        {
            var container = new ObjectMapper();
            container.Configure<Role, RoleEntity>().CreateWith(source => new RoleEntity())
                .MapMember(target => target.LoweredRoleName, source => source.RoleName.ToLower());
            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Manager",
                Description = "Department or group manager"
            };
            var entity = container.Map<Role, RoleEntity>(role);
            Assert.Equal(entity.RoleId, role.RoleId);
            Assert.Equal(entity.RoleName, role.RoleName);
            Assert.Equal(entity.LoweredRoleName, role.RoleName.ToLower());
            Assert.Equal(entity.Description, role.Description);
        }

        [Fact]
        public void TestCustomMapper()
        {
            var container = new ObjectMapper();
            container.Configure<Role, RoleEntity>().MapWith((source, target) =>
            {
                target.RoleId = source.RoleId;
                target.RoleName = source.RoleName.ToUpper();
                target.LoweredRoleName = source.RoleName.ToLower();
                target.Description = source.Description;
            });
            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Manager",
                Description = "Department or group manager"
            };
            var entity = container.Map<Role, RoleEntity>(role);
            Assert.Equal(entity.RoleId, role.RoleId);
            Assert.Equal(entity.RoleName, role.RoleName.ToUpper());
            Assert.Equal(entity.LoweredRoleName, role.RoleName.ToLower());
            Assert.Equal(entity.Description, role.Description);
        }

        [Fact]
        public void TestCustomConvension()
        {
            var container = new ObjectMapper();
            container.Conventions.Add(
                context =>
                {
                    foreach (
                        var targetMember in
                            context.TargetMembers.Where(targetMember => targetMember.MemberName.StartsWith("Lowered")))
                    {
                        if (targetMember.MemberType != typeof (string)) return;
                        Func<string, string> converter = null;
                        string prefix = null;
                        if (targetMember.MemberName.StartsWith("Lowered"))
                        {
                            prefix = "Lowered";
                            converter = source => source == null ? null : source.ToLower();
                        }
                        else if (targetMember.MemberName.StartsWith("Uppered"))
                        {
                            prefix = "Uppered";
                            converter = source => source == null ? null : source.ToUpper();
                        }
                        if (string.IsNullOrEmpty(prefix)) return;
                        var sourceName = targetMember.MemberName.Substring(prefix.Length);
                        if (!string.IsNullOrEmpty(sourceName))
                        {
                            var sourceMember = context.SourceMembers[sourceName];
                            if (sourceMember != null && sourceMember.MemberType == typeof (string))
                            {
                                context.Mappings.Set(sourceMember, targetMember).ConvertWith(converter);
                            }
                        }
                    }
                });
            var role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Manager",
                Description = "Department or group manager"
            };
            var entity = container.Map<Role, RoleEntity>(role);
            Assert.Equal(entity.RoleId, role.RoleId);
            Assert.Equal(entity.RoleName, role.RoleName);
            Assert.Equal(entity.LoweredRoleName, role.RoleName.ToLower());
            Assert.Equal(entity.Description, role.Description);
        }

        [Fact]
        public void TestShadowMap()
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                OrderCode = "A001",
                Address = "China",
                Items =
                {
                    new OrderItem
                    {
                        OrderItemId = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        Quantity = 2
                    }
                }
            };
            var entity = Mapper.Map<Order, OrderEntity>(order);
            Assert.NotNull(entity);
            Assert.Equal(order.OrderId, entity.OrderId);
            Assert.Equal(order.CustomerId, entity.CustomerId);
            Assert.Equal(order.OrderCode, entity.OrderCode);
            Assert.Equal(order.Address, entity.Address);
            Assert.Null(entity.Items);
        }

        [Fact]
        public void TestHierarchyMap()
        {
            var container = new ObjectMapper();
            container.Configure<Order, OrderEntity>().WithOptions(MemberMapOptions.Hierarchy);
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                OrderCode = "A001",
                Address = "China",
                Items =
                {
                    new OrderItem
                    {
                        OrderItemId = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        Quantity = 2
                    }
                }
            };
            var entity = container.Map<Order, OrderEntity>(order);
            Assert.NotNull(entity);
            Assert.Equal(order.OrderId, entity.OrderId);
            Assert.Equal(order.CustomerId, entity.CustomerId);
            Assert.Equal(order.OrderCode, entity.OrderCode);
            Assert.Equal(order.Address, entity.Address);
            Assert.NotNull(entity.Items);
            Assert.Equal(1, entity.Items.Count);
            var item = entity.Items.First();
            Assert.NotNull(item);
            Assert.Equal(order.Items[0].OrderItemId, item.OrderItemId);
            Assert.Equal(order.Items[0].ProductId, item.ProductId);
            Assert.Equal(order.Items[0].Quantity, item.Quantity);
        }
    }
}
