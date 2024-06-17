﻿using EmployeeDirectory.BAL.Interfaces;
using EmployeeDirectory.DAL.Interfaces;
using EmployeeDirectory.DAL.Models;

namespace EmployeeDirectory.BAL.Providers
{
    public class ProjectsProvider(IProjectRepository ProjectRepository) : IProjectProvider
    {
        public static Dictionary<int, string> Projects = new();
        private readonly IProjectRepository _ProjectRepository = ProjectRepository;

        public async Task GetProjects()
        {
            List<Project> projects = await _ProjectRepository.GetAll();
            foreach (Project project in projects)
            {
                Projects.Add(project.Id, project.Name);
            }
        }
    }
}
