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


namespace pivotal.DAL.Interfaces
{
    public interface IProjectDAL
    {
        public Task<ProjectDto> GetProjectById(int projectId, int userId);
        public Task<int> AddProject(string name, bool isPublic, int ownerId);
        public Task<bool> DeleteProject(int projectId);
        public Task<bool> UpdateProject(int id, string name, bool isPublic);
        public Task<List<ProjectDto>> GetProjectsByUserId(int userId);
        public Task<string> AddUserToProject(UserProjectDto request);
    }
}