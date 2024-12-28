using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.Business.Concrete
{
    public class CategoryManager : ICategoryDal
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public void Create(Category entity)
        {
            _categoryDal.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryDal.Delete(entity);
        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            _categoryDal.DeleteFromCategory(categoryId, productId);
        }

        public List<Category> GetAll(Expression<Func<Category, bool>> filter = null)
        {
           return _categoryDal.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryDal.GetById(id);
        }

        public Category GetByIdWithProducts(int id)
        {
           return _categoryDal.GetByIdWithProducts(id);
        }

        public Category GetOne(Expression<Func<Category, bool>> filter = null)
        {
            return _categoryDal.GetOne();
        }

        public void Update(Category entity)
        {
            _categoryDal.Update(entity);
        }
    }
}
