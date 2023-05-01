using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MRA_Jobs.Application.Abstractions;
using MRA_Jobs.Application.Common.Models;
using MRA_Jobs.Infrastructure.Persistence;

namespace MRA_Jobs.Infrastructure.Services;
public class EntityService<TEntity> : IEntityService<TEntity>
    where TEntity : class
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EntityService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ServiceResponse<TGetEntity>> Add<TAddEntity, TGetEntity>(TAddEntity entity)
    {
        var response = new ServiceResponse<TGetEntity>();
        try
        {
            var addEntity = _mapper.Map<TEntity>(entity);
            var newEntity = _context.Set<TEntity>().Add(addEntity);
            await _context.SaveChangesAsync();

            response.Data = _mapper
                .Map<TGetEntity>(newEntity.Entity);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<bool>> Delete(long id)
    {
        var response = new ServiceResponse<bool>();
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.Message = $"{nameof(TEntity)} not found";
            }
            else
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<List<TGetEntity>>> GetAll<TGetEntity>()
    {
        var response = new ServiceResponse<List<TGetEntity>>();
        try
        {
            var entity = await _context.Set<TEntity>().ToListAsync();
            if (entity == null)
            {
                response.Success = false;
                response.Message = $"{nameof(TEntity)} not found";
            }
            else
            {
                response.Data = _mapper.Map<List<TGetEntity>>(entity);
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<TGetEntity>> GetById<TGetEntity>(long id)
    {
        var response = new ServiceResponse<TGetEntity>();
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                response.Success = false;
                response.Message = $"{nameof(TEntity)} not found";
            }
            else
            {
                response.Data = _mapper.Map<TGetEntity>(entity);
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<TGetEntity>> Update<TUpdateEntity, TGetEntity>(TUpdateEntity entity)
    {
        var response = new ServiceResponse<TGetEntity>();
        try
        {

            var updatedEntity = _mapper.Map<TEntity>(entity);
            var newEntity = _context.Set<TEntity>()
                .Attach(updatedEntity)
                .State = EntityState.Modified;
            response.Data = _mapper.Map<TGetEntity>(entity);

        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
