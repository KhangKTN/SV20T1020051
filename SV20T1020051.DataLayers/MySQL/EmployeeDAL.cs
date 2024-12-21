using System;
using Dapper;
using SV20T1020051.DomainModels;

namespace SV20T1020051.DataLayers.MySQL
{
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                            insert into Employees(FullName,BirthDate,Address,Phone,Email,Photo,IsWorking)
                            values(@fullName,@birthDate,@address,@phone,@email,@photo,@isWorking);
                            select @@identity;
                           ";
                var parameters = new
                {
                    fullName = data.FullName ?? "",
                    birthDate = data.BirthDate,
                    address = data.Address ?? "",
                    phone = data.Phone ?? "",
                    email = data.Email ?? "",
                    photo = data.Photo,
                    isWorking = data.IsWorking
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
                var sql = @"select count(*) from Employees 
                    where (@searchValue = N'') or (FullName like @searchValue)";
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
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Employees where EmployeeID = @employeeId";
                var parameters = new
                {
                    employeeId = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Employees where EmployeeID = @employeeId";
                var parameters = new
                {
                    employeeId = id
                };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool IsUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where EmployeeID = @employeeId)
                                select 1
                            else 
                                select 0";
                var parameters = new { employeeId = id };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select  *
                from
                (
                    select *, row_number() over (order by FullName) as RowNumber
                    from Employees
                    where (@searchValue = N'') or (FullName like @searchValue)
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

                data = connection.Query<Employee>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            //Console.WriteLine(DateTime.Parse(data.BirthDate).);
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Employees where EmployeeID <> @employeeId and Email = @email)
                                begin
                                    update Employees 
                                    set FullName = @fullName,
                                        BirthDate = @birthDate,
                                        Address = @address,
                                        Phone = @phone,
                                        Email = @email,
                                        Photo = @photo,
                                        IsWorking = @isWorking
                                    where EmployeeID = @employeeId
                                end";
                var parameters = new
                {
                    employeeId = data.EmployeeID,
                    fullName = data.FullName ?? "",
                    birthDate = data.BirthDate,
                    address = data.Address ?? "",
                    phone = data.Phone ?? "",
                    email = data.Email ?? "",
                    photo = data.Photo,
                    isWorking = data.IsWorking
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}

