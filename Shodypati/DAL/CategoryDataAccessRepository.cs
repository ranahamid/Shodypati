using Microsoft.Practices.Unity;
using Shodypati.Controllers;
using Shodypati.Filters;
using Shodypati.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shodypati.DAL
{
    [ExceptionHandlerAttribute]
    public class CategoryDataAccessRepository : BaseController, ICategoryAccessRepository<Category, int>
    {
        public IEnumerable<Category> Get()
        {
            List<Category> entities = new List<Category>();

            entities = Db.CategoryTbls.Select(x => new Category()
            {
                Id = x.Id,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                ImagePath =  HttpUtility.UrlPathEncode(baseUrl + x.ImagePath) ,
                RawDBImagePath = x.ImagePath,
                Parent1Id = x.Parent1Id,
                //  Parent1Name_English    = GetParentNameFromAllCategories(x.Parent1Id),
                //  Parent1Name_Bangla     = GetParentNameFromAllCategories(x.Parent1Id),   
                ShowOnHomePage = x.ShowOnHomePage,
                IncludeInTopMenu = x.IncludeInTopMenu,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published,

            }).OrderBy(x => x.Name_English).ToList();

            return entities;
        }

        public Category Get(int id)
        {
            Category entity = Db.CategoryTbls.Where(x => x.Id == id).Select(x => new Category()
            {
                Id = x.Id,
                Name_English = x.Name_English,
                Name_Bangla = x.Name_Bangla,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                ImagePath = HttpUtility.UrlPathEncode(baseUrl + x.ImagePath),
                RawDBImagePath = x.ImagePath,
                Parent1Id = x.Parent1Id,
                Parent1Name_English = GetParentNameFromAllCategories(x.Parent1Id),
                Parent1Name_Bangla = GetParentNameFromAllCategories(x.Parent1Id),
                ShowOnHomePage = x.ShowOnHomePage,
                IncludeInTopMenu = x.IncludeInTopMenu,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                Published = x.Published,

            }).FirstOrDefault();

            return entity;
        }



        public void Post(Category entity)
        {
            var imgAddress = string.Empty;
            if (entity.ImagePath != null)
            {
                imgAddress = entity.ImagePath.TrimStart('/');
            }

            Db.CategoryTbls.InsertOnSubmit(new CategoryTbl
            {
                //   Id                     = entity.Id                   ,
                Name_English = entity.Name_English,
                Name_Bangla = entity.Name_Bangla,
                Description = entity.Description,
                DisplayOrder = entity.DisplayOrder,
                ImagePath = imgAddress,
                Parent1Id = entity.SelectedParent1Id,

                ShowOnHomePage = entity.ShowOnHomePage,
                IncludeInTopMenu = entity.IncludeInTopMenu,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now,
                Published = entity.Published,

            });
            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        public void Put(int id, Category entity)
        {
            var isEntity = from x in Db.CategoryTbls
                           where x.Id == entity.Id
                           select x;

            var imgAddress = string.Empty;
            if (entity.RawDBImagePath != null)
            {
                imgAddress = entity.RawDBImagePath.TrimStart('/');
            }



            CategoryTbl entitySingle = isEntity.Single();


            entitySingle.Name_English = entity.Name_English;
            entitySingle.Name_Bangla = entity.Name_Bangla;
            entitySingle.Description = entity.Description;
            entitySingle.DisplayOrder = entity.DisplayOrder;
            entitySingle.ImagePath = imgAddress;
            entitySingle.Parent1Id = entity.SelectedParent1Id;
            entitySingle.ShowOnHomePage = entity.ShowOnHomePage;
            entitySingle.IncludeInTopMenu = entity.IncludeInTopMenu;
            entitySingle.UpdatedOnUtc = DateTime.Now;
            entitySingle.Published = entity.Published;


            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }



        public void Delete(int id)
        {
            //check if it is parent of any other 
            if (IsParentOfAnyCategory(id))
            {
                return;
            }

            var query = from x in Db.CategoryTbls
                        where x.Id == id
                        select x;

            if (query.Count() == 1)
            {
                CategoryTbl entity = query.FirstOrDefault();
                Db.CategoryTbls.DeleteOnSubmit(entity ?? throw new InvalidOperationException());
            }

            try
            {
                Db.SubmitChanges();
            }
            catch (Exception)
            {
                throw new Exception("Exception");
            }
        }

        //custom

        public List<SelectListItem> GetAllCategoriesSelectList()
        {
            return AllCategories;
        }



    }
}