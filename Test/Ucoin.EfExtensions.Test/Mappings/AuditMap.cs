﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ucoin.EfExtensions.Test
{
    public partial class AuditMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AuditData>
    {
        public AuditMap()
        {
            // table
            ToTable("Audit", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.Date)
                .HasColumnName("Date")
                .IsRequired();
            Property(t => t.UserId)
                .HasColumnName("UserId")
                .IsOptional();
            Property(t => t.TaskId)
                .HasColumnName("TaskId")
                .IsOptional();
            Property(t => t.Content)
                .HasColumnName("Content")
                .IsRequired();
            Property(t => t.Username)
                .HasColumnName("Username")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();
            Property(t => t.RowVersion)
                .HasColumnName("RowVersion")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasMaxLength(8)
                .IsRowVersion()
                .IsRequired();

            // Relationships
            HasOptional(t => t.Task)
                .WithMany(t => t.Audits)
                .HasForeignKey(d => d.TaskId)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.User)
                .WithMany(t => t.Audits)
                .HasForeignKey(d => d.UserId)
                .WillCascadeOnDelete(false);

        }
    }
}
