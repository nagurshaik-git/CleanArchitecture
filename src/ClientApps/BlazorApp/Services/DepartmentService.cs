﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp.Extensions;
using BlazorApp.ViewModels.DepartmentsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace BlazorApp.Services
{
    [SingletonService]
    public class DepartmentService
    {
        private readonly HttpClient _httpClient;

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Not applicable for constructor")]
        public DepartmentService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
        }

        public async Task<List<DepartmentDetailsViewModel>> GetListAsync()
        {
            List<DepartmentDetailsViewModel> departments = await _httpClient.GetFromJsonAsync<List<DepartmentDetailsViewModel>>("v1/departments");
            return departments;
        }

        public async Task<List<SelectListItem>> GetSelectListAsync(int? selectedDepartment = null)
        {
            List<SelectListItem> departments = await _httpClient
                .GetFromJsonAsync<List<SelectListItem>>($"v1/departments/select-list?selectedDepartment={selectedDepartment}");
            return departments;
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateDepartmentViewModel createDepartmentViewModel)
        {
            createDepartmentViewModel.ThrowIfNull(nameof(createDepartmentViewModel));

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("v1/departments", createDepartmentViewModel);
            return response;
        }

        public async Task<DepartmentDetailsViewModel> GetByIdAsync(int departmentId)
        {
            departmentId.ThrowIfNotPositive(nameof(departmentId));

            DepartmentDetailsViewModel response = await _httpClient.GetFromJsonAsync<DepartmentDetailsViewModel>($"v1/departments/{departmentId}");
            return response;
        }

        public async Task<HttpResponseMessage> UpdateAsync(UpdateDepartmentViewModel updateDepartmentViewModel)
        {
            updateDepartmentViewModel.ThrowIfNull(nameof(updateDepartmentViewModel));

            HttpResponseMessage response = await _httpClient
                .PutAsJsonAsync($"v1/departments/{updateDepartmentViewModel.DepartmentId}", updateDepartmentViewModel);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(int departmentId)
        {
            departmentId.ThrowIfNotPositive(nameof(departmentId));

            HttpResponseMessage response = await _httpClient.DeleteAsync($"v1/departments/{departmentId}");

            return response;
        }
    }
}
