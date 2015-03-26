using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using Ucoin.Framework.EfExtensions.Batch;
using Ucoin.Framework.EfExtensions.Mapping;

namespace Ucoin.Framework.EfExtensions.Batch
{
    internal class BatchInsertProvider : IBatchInsert
    {
        public void Insert<T>(DbContext dbContext, IEnumerable<T> entities, int batchSize)
            where T : class
        {           
            var cs = ConfigurationManager.ConnectionStrings[dbContext.GetType().Name];
            var entiyMap = dbContext.GetEntityMap<T>(typeof(T));

            using (var dbConnection = new SqlConnection(cs.ConnectionString))
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        Insert(entities, transaction, entiyMap, batchSize);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        throw ex;
                    }
                }
            }
        }

        private void Insert<T>(IEnumerable<T> entities, SqlTransaction transaction, EntityMap entiyMap, int batchSize)
        {
            var options = SqlBulkCopyOptions.Default;

            using (DataTable dataTable = CreateDataTable(entiyMap, entities))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(transaction.Connection, options, transaction))
                {
                    sqlBulkCopy.BatchSize = batchSize;
                    sqlBulkCopy.DestinationTableName = dataTable.TableName;
                    sqlBulkCopy.WriteToServer(dataTable);
                }
            }
        }

        private static DataTable CreateDataTable<T>(EntityMap entiyMap, IEnumerable<T> entities)
        {
            var dataTable = BuildDataTable<T>(entiyMap);

            foreach (var entity in entities)
            {
                DataRow row = dataTable.NewRow();

                foreach (var column in entiyMap.PropertyMaps)
                {
                    var @value = entity.GetPropertyValue(column.PropertyName);

                    if (column.IsIdentity) continue;

                    if (@value == null)
                    {
                        row[column.ColumnName] = DBNull.Value;
                    }
                    else
                    {
                        row[column.ColumnName] = @value;
                    }
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private static DataTable BuildDataTable<T>(EntityMap entiyMap)
        {
            var entityType = typeof(T);
            string tableName = string.Join(@".", entiyMap.SchemaName, entiyMap.TableName);

            var dataTable = new DataTable(tableName);
            var primaryKeys = new List<DataColumn>();

            foreach (var columnMapping in entiyMap.PropertyMaps)
            {
                var propertyInfo = entityType.GetProperty(columnMapping.PropertyName, '.');
                columnMapping.Type = propertyInfo.PropertyType;

                var dataColumn = new DataColumn(columnMapping.ColumnName);

                Type dataType;
                if (propertyInfo.PropertyType.IsNullable(out dataType))
                {
                    dataColumn.DataType = dataType;
                    dataColumn.AllowDBNull = true;
                }
                else
                {
                    dataColumn.DataType = propertyInfo.PropertyType;
                    dataColumn.AllowDBNull = columnMapping.Nullable;
                }

                if (columnMapping.IsIdentity)
                {
                    dataColumn.Unique = true;
                    if (propertyInfo.PropertyType == typeof(int)
                      || propertyInfo.PropertyType == typeof(long))
                    {
                        dataColumn.AutoIncrement = true;
                    }
                    else continue;
                }
                else
                {
                    dataColumn.DefaultValue = columnMapping.DefaultValue;
                }

                if (propertyInfo.PropertyType == typeof(string))
                {
                    dataColumn.MaxLength = columnMapping.MaxLength;
                }

                if (columnMapping.IsPk)
                {
                    primaryKeys.Add(dataColumn);
                }

                dataTable.Columns.Add(dataColumn);
            }

            dataTable.PrimaryKey = primaryKeys.ToArray();

            return dataTable;
        }
    }
}
