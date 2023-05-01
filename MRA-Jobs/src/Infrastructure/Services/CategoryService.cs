using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MRA_Jobs.Application.Common.Interfaces;
using MRA_Jobs.Application.Common.Models;
using MRA_Jobs.Domain.Entities;
using MRA_Jobs.Infrastructure.Persistence;

namespace MRA_Jobs.Infrastructure.Services;
public class CategoryService : EntityService<Category>, ICategoryService
{
    private ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context,IMapper mapper)
        :base(context,mapper)
    {
        _context = context;
    }
    //public async Task<ServiceResponse<Category>> Add(Category category)
    //{
    //    ServiceResponse<Category> response = new ServiceResponse<Category>();
    //    try
    //    {
    //       var result = await _context.Categories.AddAsync(category);
    //        response.Data = result.Entity;
    //    }
    //    catch (Exception ex)
    //    {
    //        response.Message = ex.Message;
    //        response.Success = false;
    //    }
    //    return response;

    //}

    //public async Task<ServiceResponse<bool>> Delete(int id)
    //{
    //    ServiceResponse<bool> response = new ServiceResponse<bool>();
    //    try
    //    {
    //        var category = await _context.Categories.FindAsync(id);
    //        if (category != null)
    //        {
    //           response.Data = true;
    //           _context.Categories.Remove(category);
    //        }
    //        else
    //        {
    //            response.Data = false;
    //            response.Message = "Category not found";
    //            response.Success = false;
    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //        response.Success = false;
    //        response.Message = ex.Message;
    //    }
    //    return response;
    //}

    //public async Task<ServiceResponse<List<Category>>> Get()
    //{
    //    ServiceResponse<List<Category>> response = new ServiceResponse<List<Category>>();
    //    try
    //    {
    //        var categories = await _context.Categories.Include(s=>s.JobVacancies).Include(s=>s.EducationVacancies).ToListAsync();
    //        if (!categories.Any())
    //        {
    //            response.Success = false;
    //            response.Message = "Category is Empty";
    //        }
    //        else
    //        {
    //            response.Data = categories;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        response.Message = ex.Message;
    //        response.Success = false;
    //    }
    //    return response;
    //}

    //public async Task<ServiceResponse<Category>> GetById(int id)
    //{
    //    var response = new ServiceResponse<Category>();
    //    try
    //    {
    //        var category = await _context.Categories.FindAsync(id);
    //        if (category ==null)
    //        {
    //            response.Success =false;
    //            response.Message = "Category not found";
    //        }
    //        else
    //        {
    //            response.Data = category;
    //        }
            
    //    }
    //    catch (Exception ex)
    //    {

    //       response.Message=ex.Message;
    //       response.Success = false;
    //    }
    //    return response;
    //}

    //public async Task<ServiceResponse<Category>> Update(Category category)
    //{
    //    var response = new ServiceResponse<Category>();
    //    try
    //    {
    //        var updatedCategory = _context.Categories
    //            .Attach(category)
    //            .State = EntityState.Modified;

    //        response.Data = category;
    //    }
    //    catch (Exception ex)
    //    {
    //        response.Success =false;
    //        response.Message = ex.Message;
    //    }
    //    return response;
    //}
}
