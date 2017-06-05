﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SuiBao_v1.Repository
{
    public abstract class BaseRepository<TModel> : SuiBao_v1.IRepository.IBaseRepository<TModel> where TModel : class
    {
        /// <summary>
        /// EF 容器对象
        /// </summary>
        private readonly DbContext _db;//= SuiBao.Models.OperationContext.Current;

        /// <summary>
        /// 数据集
        /// </summary>
        private readonly DbSet<TModel> _dbSet;

        protected BaseRepository(DbContext context)
        {
            _db = context;
            //根据传入的 实体类，返回 对应的 EF 数据集对象，可用来操作 实体类 对应的 数据表
            _dbSet = _db.Set<TModel>();
        }

        #region  新增实体
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TModel entity)
        {
            _dbSet.Add(entity);
        }
        public void AddRange(IEnumerable<TModel> entity)
        {
            _dbSet.AddRange(entity);
        }
        #endregion

        #region  删除实体
        /// <summary>
        /// 2.0 删除实体
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TModel entity)
        {
            //手动附加，调用删除方法改变 其 标识
            //将 对象 加入到 EF容器中
            _dbSet.Attach(entity);
            //将 对象 标记为 删除状态
            _dbSet.Remove(entity);
        }
        #endregion

        #region 根据条件删除
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="where"></param>
        public void DeleteBy(Expression<Func<TModel, bool>> where)
        {
            //根据条件 使用 EF 将要删除的对象 查询出来，并存入 EF容器
            var list = _dbSet.Where(where);
            //循环对象，并修改对象的 状态为“删除”
            foreach (var item in list)
            {
                _dbSet.Remove(item);
            }
        }
        #endregion

        #region  修改指定对象
        /// <summary>
        /// 3.0 修改指定对象
        /// </summary>
        /// <param name="entity"></param>     
        /// <param name="modifiedPropertyNames">修改了的属性的名字，这些属性的值需要更新到数据库中</param>
        public void Modify(TModel entity, params Expression<Func<TModel, object>>[] modifiedPropertyNames)
        {
            //1.将对象 加入 到EF容器中，并获取 Entry对象（包装器）
            DbEntityEntry<TModel> entry = _db.Entry(entity);
            //2.修改属性状态
            _dbSet.Attach(entity);
            if (modifiedPropertyNames.Length > 0)
            {
                foreach (var proName in modifiedPropertyNames)
                {
                    entry.Property(proName).IsModified = true;
                }
            }
        }

        public void Modify(TModel entity, params string[] modifiedPropertyNames)
        {
            //1.将对象 加入 到EF容器中，并获取 Entry对象（包装器）
            DbEntityEntry<TModel> entry = _db.Entry(entity);
            _dbSet.Attach(entity);
            //  SaveChange的时候，EF会找出 IsModified =true的属性，并把他们生成到 update　语句中  
            foreach (var proName in modifiedPropertyNames)
            {
                entry.Property(proName).IsModified = true;
            }
        }
        #endregion

        #region  根据条件修改

        /// <summary>
        ///  根据条件修改
        /// </summary>
        /// <param name="where">修改条件（哪些要修改）</param>
        /// <param name="updater"></param>
        public void ModifyBy(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TModel>> updater)
        {
            //获取Update的赋值语句
            var updateMemberExpr = (MemberInitExpression)updater.Body;
            var updateMemberCollection = updateMemberExpr.Bindings.Cast<MemberAssignment>().Select(c =>
            new
            {
                c.Member.Name,
                Value = c.Expression.NodeType == ExpressionType.Constant ? ((ConstantExpression)c.Expression).Value : Expression.Lambda(c.Expression, null).Compile().DynamicInvoke()
            }).ToArray();
            string[] modifyPropertyNames = updateMemberCollection.Select(c => c.Name).ToArray();
            object[] modifyPropertyValues = updateMemberCollection.Select(c => c.Value).ToArray();

            Type typeObj = typeof(TModel);
            //获取要修改的 属性类型 对象 PropertyInfo
            List<PropertyInfo> listModifyProperty = new List<PropertyInfo>();
            foreach (var propertyName in modifyPropertyNames)
            {
                //将要修改的 属性类型对象 存入集合
                listModifyProperty.Add(typeObj.GetProperty(propertyName));
            }
            var listModifing = _dbSet.Where(where);
            foreach (var item in listModifing)
            {
                //遍历要修改的 属性类型 集合
                for (int index = 0; index < listModifyProperty.Count; index++)
                {
                    PropertyInfo proModify = listModifyProperty[index];
                    proModify.SetValue(item, modifyPropertyValues[index], null);
                }
            }
        }

        public void ModifyBy(Expression<Func<TModel, bool>> where, string[] modifyPropertyNames, object[] modifyPropertyValues)
        {
            //获取 要修改的 实体类 的 类型对象Type
            Type typeObj = typeof(TModel);
            //获取要修改的 属性类型 对象 PropertyInfo
            List<PropertyInfo> listModifyProperty = new List<PropertyInfo>();
            foreach (var propertyName in modifyPropertyNames)
            {
                //将要修改的 属性类型对象 存入集合
                listModifyProperty.Add(typeObj.GetProperty(propertyName));
            }
            //查询要修改的对象(注意：对象已经存在EF容器中了)
            var listModifing = _dbSet.Where(where);
            //循环修改这些对象的属性
            foreach (var item in listModifing)
            {
                //遍历要修改的 属性类型 集合
                for (int index = 0; index < listModifyProperty.Count; index++)
                {
                    PropertyInfo proModify = listModifyProperty[index];
                    //修改 item 对象 的 proModify对应属性的 值 为 数组modifyPropertyValues里相同下标的值！
                    proModify.SetValue(item, modifyPropertyValues[index], null);
                }
            }
        }
        #endregion



        #region  根据条件查询单个实体
        /// <summary>
        /// 4.0 根据条件查询单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TModel Single(Expression<Func<TModel, bool>> where)
        {
            return _dbSet.FirstOrDefault(where);
        }

        public async Task<TModel> SingleAsync(Expression<Func<TModel, bool>> where)
        {
            return await _dbSet.FirstOrDefaultAsync(where);
        }
        #endregion

        #region  根据条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <returns></returns>
        public IQueryable<TModel> Where(Expression<Func<TModel, bool>> where)
        {
            return _dbSet.Where(where);
        }
        #endregion


        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="where">查询条件表达式</param>
        /// <returns></returns>
        public int Count(Expression<Func<TModel, bool>> where)
        {
            return _dbSet.Count(where);
        }
        public async Task<int> CountAsync(Expression<Func<TModel, bool>> where)
        {
            return await _dbSet.CountAsync(where);
        }

        #region 根据 条件 查询 和 排序
        /// <summary>
        /// 根据 条件 查询 和 排序
        /// </summary>
        /// <typeparam name="TOrderKey">排序属性类型</typeparam>
        /// <param name="where">查询条件表达式</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public IQueryable<TModel> Where<TOrderKey>(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TOrderKey>> orderBy, bool isAsc = true)
        {
            if (isAsc)
                return _dbSet.Where(where).OrderBy(orderBy);
            else
                return _dbSet.Where(where).OrderByDescending(orderBy);
        }
        #endregion


        /// <summary>
        /// 根据 条件 查询 和 排序，并使用include预加载
        /// </summary>
        /// <typeparam name="TOrderKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <param name="includeNames">要连接查询的 导航属性名称</param>
        /// <returns></returns>
        public IQueryable<TModel> WhereInclude<TOrderKey>(Expression<Func<TModel, bool>> where, Expression<Func<TModel, TOrderKey>> orderBy, bool isAsc = true, params string[] includeNames)
        {
            DbQuery<TModel> dbQuery = null;
            //1.先获取 Include 之后的 DBQuery对象
            foreach (string includePropertyName in includeNames)
            {
                //1.1如果 第一次 Include，则调用 dbSet的 Include 方法
                if (dbQuery == null)
                {
                    //将获取的 dbQuery返回，已留作 后面的 链式编程使用
                    dbQuery = _dbSet.Include(includePropertyName);
                }
                else
                {
                    //如果之前已经 Include 过了，则直接通过dbQuery完成链式编程
                    dbQuery = dbQuery.Include(includePropertyName);
                }
            }
            //2.加入 查询 和排序条件
            if (isAsc)
                return dbQuery.Where(where).OrderBy(orderBy);
            else
                return dbQuery.Where(where).OrderByDescending(orderBy);
        }

        /// <summary>
        /// 连接查询
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="inner"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public IQueryable<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TModel, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TModel, TInner, TResult>> resultSelector)
        {
            return _dbSet.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public bool Any(Expression<Func<TModel, bool>> where)
        {
            return _dbSet.Any(where);
        }

        public async Task<bool> AnyAsync(Expression<Func<TModel, bool>> where)
        {
            return await _dbSet.AnyAsync(where);
        }

    }
}
