using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using pivotal.Models;
using pivotalHeroku.Models;
using Dapper;
using pivotal.DAL.Interfaces;
using Microsoft.Extensions.Options;
using pivotalHeroku;

namespace pivotal.DAL
{
    public class UserDAL : IUserDAL
    {
        IOptions<PivotalConfiguration> _options;
        public UserDAL(IOptions<PivotalConfiguration> options)
        {
            _options = options;
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var d = _options.Value;
            string sql = $"SELECT * FROM {_options.Value.Schema}.User WHERE Id = @id";
            try
            {
                using (IDbConnection con = new MySqlConnection(_options.Value.ConnectionString))
                {
                    var UserList = await con.QueryAsync<UserDto>(sql, new { id });

                    return UserList.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<UserDto> GetUserByEmail(string email)
        {
            string sql = $"SELECT * FROM {_options.Value.Schema}.User WHERE Email = @email";
            try
            {
                using (IDbConnection con = new MySqlConnection(_options.Value.ConnectionString))
                {
                    var UserList = await con.QueryAsync<UserDto>(sql, new { email });

                    return UserList.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<int> CreateUser(UserDto user)
        {
            string sql = $@"INSERT INTO {_options.Value.Schema}.User(Name, Email, Password)
                            VALUES(@name, @email, @password);
                            SELECT LAST_INSERT_ID();";
            try
            {
                using (IDbConnection con = new MySqlConnection(_options.Value.ConnectionString))
                {
                    int id = await con.ExecuteScalarAsync<int>(
                        sql,
                        new
                        {
                            @name = user.Name,
                            @email = user.Email,
                            @password = user.Password
                        }
                    );
                    return id;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicate entry"))
                {
                    return -1;
                }
                return 0;
            }
        }

        public async Task<Tuple<ProjectDto, List<UserDto>>> GetUsersByProjectId(UserProjectDto dto)
        {
            try
            {
                string usersQuery = $@"SELECT u.Id, u.Name, u.Email from {_options.Value.Schema}.UserProjectMapping m
                                    INNER JOIN pivotal.User u ON u.id = m.userId
                                    WHERE m.projectId = @projectId;";
                string projectQuery = $@"SELECT IsPublic, OwnerId FROM {_options.Value.Schema}.Project WHERE id = @projectId LIMIT 1";
                using (IDbConnection con = new MySqlConnection(_options.Value.ConnectionString))
                {
                    Task<ProjectDto> t1 = con.QueryFirstOrDefaultAsync<ProjectDto>(projectQuery, new { @projectId = dto.ProjectId });
                    Task<IEnumerable<UserDto>> t2 = con.QueryAsync<UserDto>(usersQuery, new { @projectId = dto.ProjectId });

                    await Task.WhenAll(t1, t2);

                    ProjectDto projectDetails = await t1;
                    List<UserDto> users = (await t2).ToList();
                    return new Tuple<ProjectDto, List<UserDto>>(projectDetails, users);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}