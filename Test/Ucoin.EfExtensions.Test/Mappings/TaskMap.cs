﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ucoin.EfExtensions.Test
{
    public partial class TaskMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Task>
    {
        public TaskMap()
        {
            // table
            ToTable("Task", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.StatusId)
                .HasColumnName("StatusId")
                .IsRequired();
            Property(t => t.PriorityId)
                .HasColumnName("PriorityId")
                .IsOptional();
            Property(t => t.CreatedId)
                .HasColumnName("CreatedId")
                .IsRequired();
            Property(t => t.Summary)
                .HasColumnName("Summary")
                .HasMaxLength(255)
                .IsRequired();
            Property(t => t.Details)
                .HasColumnName("Details")
                .HasMaxLength(2000)
                .IsOptional();
            Property(t => t.StartDate)
                .HasColumnName("StartDate")
                .IsOptional();
            Property(t => t.DueDate)
                .HasColumnName("DueDate")
                .IsOptional();
            Property(t => t.CompleteDate)
                .HasColumnName("CompleteDate")
                .IsOptional();
            Property(t => t.AssignedId)
                .HasColumnName("AssignedId")
                .IsOptional();
            Property(t => t.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();
            Property(t => t.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired();
            Property(t => t.RowVersion)
                .HasColumnName("RowVersion")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasMaxLength(8)
                .IsRowVersion()
                .IsRequired();
            Property(t => t.LastModifiedBy)
                .HasColumnName("LastModifiedBy")
                .HasMaxLength(50)
                .IsOptional();

            // Relationships
            HasOptional(t => t.Priority)
                .WithMany(t => t.Tasks)
                .HasForeignKey(d => d.PriorityId)
                .WillCascadeOnDelete(false);
            HasRequired(t => t.Status)
                .WithMany(t => t.Tasks)
                .HasForeignKey(d => d.StatusId)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.AssignedUser)
                .WithMany(t => t.AssignedTasks)
                .HasForeignKey(d => d.AssignedId)
                .WillCascadeOnDelete(false);
            HasRequired(t => t.CreatedUser)
                .WithMany(t => t.CreatedTasks)
                .HasForeignKey(d => d.CreatedId)
                .WillCascadeOnDelete(false);

        }
    }
}
