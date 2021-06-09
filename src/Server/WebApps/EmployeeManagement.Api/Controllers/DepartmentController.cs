﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Api.ApiModels.DepartmentModels;
using EmployeeManagement.Api.AutoMapper;
using EmployeeManagement.Application.Dtos.DepartmentDtos;
using EmployeeManagement.Application.Infrastrucures;
using EmployeeManagement.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IExceptionLogger _exceptionLogger;

        public DepartmentController(IDepartmentService departmentService, IExceptionLogger exceptionLogger)
        {
            _departmentService = departmentService;
            _exceptionLogger = exceptionLogger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<DepartmentDetailsDto>>> GetDepartmentList()
        {
            try
            {
                List<DepartmentDetailsDto> departmentDetailsDtos = await _departmentService.GetDepartmentListAsync();
                return departmentDetailsDtos;
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("select-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SelectList>> GetDepartmentSelectList(int? selectedDepartment)
        {
            try
            {
                SelectList selectList = await _departmentService.GetDepartmentSelectListAsync(selectedDepartment);
                return selectList;
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CreateDepartment([FromBody] CreateDepartmentModel model)
        {
            try
            {
                CreateDepartmentDto createDepartmentDto = new CreateDepartmentDto
                {
                    DepartmentName = model.DepartmentName,
                    Description = model.Description
                };

                int departmentId = await _departmentService.CreateDepartmentAsync(createDepartmentDto);
                return CreatedAtAction(nameof(GetDepartment), new { departmentId }, model);
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{departmentId:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DepartmentDetailsDto>> GetDepartment(int departmentId)
        {
            try
            {
                DepartmentDetailsDto departmentDetailsDto = await _departmentService.GetDepartmentAsync(departmentId);
                return departmentDetailsDto;
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{departmentId:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateDepartment(int departmentId, UpdateDepartmentModel model)
        {
            try
            {
                if (departmentId != model.DepartmentId)
                {
                    ModelState.AddModelError(nameof(model.DepartmentId), "The DepartmentId does not match with route value.");
                    return BadRequest(ModelState);
                }

                bool isExistent = await _departmentService.DepartmentExistsAsync(departmentId);

                if (!isExistent)
                {
                    ModelState.AddModelError(nameof(model.DepartmentId), "The Department does not exist.");
                    return BadRequest(ModelState);
                }

                UpdateDepartmentDto updateDepartmentDto = new UpdateDepartmentDto()
                {
                    DepartmentId = model.DepartmentId,
                    DepartmentName = model.DepartmentName,
                    Description = model.Description
                };

                await _departmentService.UpdateDepartmentAsync(updateDepartmentDto);
                return Ok();
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{departmentId:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            try
            {
                bool isExistent = await _departmentService.DepartmentExistsAsync(departmentId);

                if (isExistent == false)
                {
                    ModelState.AddModelError(nameof(departmentId), "The DepartmentId does not exist.");
                    return BadRequest(ModelState);
                }

                await _departmentService.DeleteDepartment(departmentId);
                return NoContent();
            }
            catch (Exception exception)
            {
                await _exceptionLogger.LogAsync(exception);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}