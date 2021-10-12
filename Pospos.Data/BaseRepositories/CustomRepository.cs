using Dapper;
using Pospos.Core.Attributes;
using Pospos.Core.Common;
using Pospos.Core.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pospos.Data.BaseRepositories
{
    public class CustomRepository
    {
        private IConnectionManager _conn;
        private const int _commandTimeout = 60;
        public CustomRepository(IConnectionManager connectionManager)
        {
            _conn = connectionManager;
        }

        protected virtual async Task<IEnumerable<TEntity>> QeryAsync<TEntity>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            using (var con = _conn.Connection)
            {
                return await con.QueryAsync<TEntity>(sql, parameters, commandType: commandType, commandTimeout: _commandTimeout);
            }
        }

        protected virtual async Task<TEntity> QeryFirstAsync<TEntity>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            using (var con = _conn.Connection)
            {
                return await con.QueryFirstOrDefaultAsync<TEntity>(sql, parameters, commandType: commandType, commandTimeout: _commandTimeout);
            }
        }

        protected virtual TEntity QeryFirst<TEntity>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            using (var con = _conn.Connection)
            {
                return con.QueryFirst<TEntity>(sql, parameters, commandType: commandType, commandTimeout: _commandTimeout);
            }
        }

        public virtual async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var columns = GetColumns<TEntity>();
            var stringOfColumns = string.Join(", ", columns.Select(x => $"[{x.Item1}]")).Replace(", [TotalRowCount]", "");
            var stringOfParameters = string.Join(", ", columns.Select(x => "@" + x.Item2)).Replace(", @TotalRowCount", "");
            var tableAttribute = typeof(TEntity).GetCustomAttribute<TableName>();
            entity.InsertDate = DateTime.Now;

            var query = $@"Insert Into [{tableAttribute.SchemeName}].[{tableAttribute.Name}] ({stringOfColumns}) VALUES ({stringOfParameters}) Select SCOPE_IDENTITY()";

            using (var con = _conn.Connection)
            {
                entity.Id = await con.QueryFirstAsync<int>(query, entity, commandType: CommandType.Text);
            }

            return entity;
        }

        public virtual async Task<bool> UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var columns = GetColumns<TEntity>();
            var tableAttribute = typeof(TEntity).GetCustomAttribute<TableName>();

            var query = GenerateUpdateQuery<TEntity>(tableAttribute.Name);

            using (var con = _conn.Connection)
            {
                return await con.ExecuteAsync(query, entity, commandType: CommandType.Text) > 0;
            }
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int Id) where TEntity : BaseEntity
        {
            var tableAttribute = typeof(TEntity).GetCustomAttribute<TableName>();
            var query = $@"Select * from [{tableAttribute.SchemeName}].[{tableAttribute.Name}] with (NOLOCK) where Id = @Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);
            using (var con = _conn.Connection)
            {
                return await con.QueryFirstOrDefaultAsync<TEntity>(query, parameters, commandType: CommandType.Text);
            }
        }


        public virtual async Task<int> ExecuteAsync(string sql, DynamicParameters parameters, CommandType commandType = CommandType.Text)
        {
            using (var con = _conn.Connection)
            {
                return await con.ExecuteAsync(sql, parameters, commandType: commandType, commandTimeout: _commandTimeout);
            }
        }

        private IEnumerable<Tuple<string, string>> GetColumns<TEntity>()
        {
            var columns = new List<Tuple<string, string>>();
            var list = typeof(TEntity).GetProperties().Where(e => e.Name != "Id");
            foreach (var column in list)
            {
                var ignore = column.GetCustomAttribute<IgnoreParameter>();
                var columnInfo = column.GetCustomAttribute<ColumnName>();

                if (ignore == null)
                {
                    if (columnInfo != null)
                    {
                        columns.Add(new Tuple<string, string>(columnInfo.Name, column.Name));
                    }
                    else
                    {
                        columns.Add(new Tuple<string, string>(column.Name, column.Name));
                    }
                }
            }
            return columns;
        }

        private string GenerateUpdateQuery<TEntity>(string _tableName)
        {
            IEnumerable<PropertyInfo> GetProperties = typeof(TEntity).GetProperties();

            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1);
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where (attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore") && prop.Name != "Id"
                    select prop.Name).ToList();
        }

        public virtual async Task<int> Delete<TEntity>(int Id)
        {
            var _tableName = typeof(TEntity).GetCustomAttribute<TableName>();

            using (var con = _conn.Connection)
            {
                return await con.ExecuteAsync($"DELETE FROM {_tableName.Name} WHERE Id = @Id", new { Id = Id });
            }
        }

    }
}
