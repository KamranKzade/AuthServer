using SharedLibrary.Dtos;
using System.Linq.Expressions;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Service.Services;

public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<TEntity> _genericRepo;


	public ServiceGeneric(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepo)
	{
		_unitOfWork = unitOfWork;
		_genericRepo = genericRepo;
	}

	public async Task<Response<TDto>> AddAsync(TDto entity)
	{
		var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
		await _genericRepo.AddAsync(newEntity);
		await _unitOfWork.CommitAsync();

		var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
		return Response<TDto>.Success(newDto, 200);
	}

	public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
	{
		var product = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepo.GetAllAsync());

		return Response<IEnumerable<TDto>>.Success(product, 200);
	}

	public async Task<Response<TDto>> GetByIdAsync(int id)
	{
		var product = await _genericRepo.GetByIdAsync(id);

		if (product == null)
		{
			return Response<TDto>.Fail("Id not fount", 404, true);
		}

		return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);
	}

	public async Task<Response<NoDataDto>> RemoveAsync(int id)
	{
		var isExistEntity = await _genericRepo.GetByIdAsync(id);

		if (isExistEntity == null)
		{
			return Response<NoDataDto>.Fail("Id not found", 404, true);
		}

		_genericRepo.Remove(isExistEntity);

		await _unitOfWork.CommitAsync();

		// 204 durum kodu => No Content => Response body'sinde hec 1 data olmayacaq
		return Response<NoDataDto>.Success(204);
	}

	public async Task<Response<NoDataDto>> UpdateAsync(TDto entity, int id)
	{
		var isExistEntity = await _genericRepo.GetByIdAsync(id);

		if (isExistEntity == null)
		{
			return Response<NoDataDto>.Fail("Id not found", 404, true);
		}

		var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

		_genericRepo.UpdateAsync(updateEntity);

		await _unitOfWork.CommitAsync();

		// 204 durum kodu => No Content => Response body'sinde hec 1 data olmayacaq
		return Response<NoDataDto>.Success(204);
	}

	public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
	{
		var list = _genericRepo.Where(predicate);

		return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
	}
}
