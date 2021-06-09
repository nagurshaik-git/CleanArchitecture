﻿using System.Threading.Tasks;
using EmployeeManagement.Application.Dtos.EmployeeDtos;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace EmployeeManagement.Application.Services
{
    [ScopedService]
    public interface IEmployeeService
    {
        Task<PaginatedList<EmployeeDetailsDto>> GetEmployeeListAsync(int pageNumber, int pageSize);

        Task<EmployeeDetailsDto> GetEmployeeDetailsAsync(int employeeId);

        Task CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);

        Task UpdateEmplyeeAsync(UpdateEmployeeDto updateEmployeeDto);

        Task DeleteEmployee(int employeeId);
    }
}