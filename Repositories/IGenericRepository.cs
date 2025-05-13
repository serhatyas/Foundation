using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    public interface IGenericRepository<T> where T: class
    {

        //performanstan ötürü IQueryable kullandık. List IEnumerable farkı yok
        IQueryable<T> GetAll();

        //Expression<Func<T, bool == p => p.Price > 100
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        //valueTask cachelerden hızlı cevap dönmesini sağlar
        //ValueTask<T?> geri dönüş tipi null olabilir
        ValueTask<T?> GetByIdAsync(int id);

        //ValueTask performanstan ötürü
        ValueTask AddAsync(T entity);

        //bu metotlar async değil çünkü dbset update ve remove işlemleri async değil
        void Update(T entity);
        void Delete(T entity);
    }
}
