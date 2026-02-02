using System.Linq.Expressions;
using AdminSystem.Models.ViewModels;

namespace AdminSystem.Data.Repositories;

/// <summary>
/// 仓储接口
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// 根据 ID 获取实体
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// 获取所有实体
    /// </summary>
    Task<List<T>> GetAllAsync();

    /// <summary>
    /// 根据条件查询
    /// </summary>
    Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// 根据条件查询单个实体
    /// </summary>
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PagedResult<T>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? predicate = null);

    /// <summary>
    /// 添加实体
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// 批量添加
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// 更新实体
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// 删除实体
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// 根据 ID 删除
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// 批量删除
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// 判断是否存在
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// 获取数量
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
}
