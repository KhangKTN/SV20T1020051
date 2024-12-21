using Dapper;
using SV20T1020051.DomainModels;
using System.Collections.Generic;


namespace SV20T1020051.DataLayers.MySQL
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            int id = 0;
            using(var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Customers where Email = @Email)
                                select -1
                            else
                                begin
                                    insert into Customers(CustomerName,ContactName,Province,Address,Phone,Email,IsLocked)
                                    values(@CustomerName,@ContactName,@Province,@Address,@Phone,@Email,@IsLocked);
                                    select @@identity;
                                end";
                var parameters = new
                {
                    CustomerName = data.CustomerName ?? "", 
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsLocked = data.IsLocked
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Customers 
                    where (@searchValue = N'') or (CustomerName like @searchValue)";
                var parameters = new
                {
                    searchValue = searchValue ?? "",

                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using(var connection = OpenConnection())
            {
                var sql = @"delete from Customers where CustomerId = @customerId";
                var parameters = new
                {
                    customerId = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Customer? Get(int id)
        {
            Customer? data = null;
            using(var connection = OpenConnection())
            {
                var sql = @"select * from Customers where CustomerId = @customerId";
                var parameters = new
                {
                    customerId = id
                };
                data = connection.QueryFirstOrDefault<Customer>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool IsUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where CustomerId = @customerId)
                                select 1
                            else 
                                select 0";
                var parameters = new {customerId = id};
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> data = new List<Customer>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select  *
                from
                (
                    select *, row_number() over (order by CustomerName) as RowNumber
                    from Customers
                    where (@searchValue = N'') or (CustomerName like @searchValue)
                ) as t                            
                where  (@pageSize = 0)
                    or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                order by RowNumber";

                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue ?? ""
                };

                data = connection.Query<Customer>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Customer data)
        {
            try
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = @"update Customers 
                                    set CustomerName = @customerName,
                                        ContactName = @contactName,
                                        Province = @province,
                                        Address = @address,
                                        Phone = @phone,
                                        Email = @email,
                                        IsLocked = @isLock
                                    where CustomerID = @customerId AND not exists(select * from Customers where CustomerID <> @customerId and Email = @email);";
                    var parameters = new
                    {
                        customerId = data.CustomerID,
                        customerName = data.CustomerName ?? "",
                        contactName = data.ContactName ?? "",
                        province = data.Province ?? "",
                        address = data.Address ?? "",
                        phone = data.Phone ?? "",
                        email = data.Email ?? "",
                        isLock = data.IsLocked
                    };
                    result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                    connection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }
    }
}
