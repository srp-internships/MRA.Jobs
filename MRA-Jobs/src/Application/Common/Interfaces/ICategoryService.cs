﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA_Jobs.Application.Abstractions;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Application.Common.Interfaces
{
    public interface ICategoryService:IEntityService<Category>
    {
        //public Task<ServiceResponse<List<Category>>> Get();
        //public Task<ServiceResponse<Category>> GetById(int id);
        //public Task<ServiceResponse<bool>> Delete(int id);
        //public Task<ServiceResponse<Category>>Update(Category category);
        //public Task<ServiceResponse<Category>> Add(Category category);
    }
}
