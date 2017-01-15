using DeerTier.Web.Data;
using DeerTier.Web.Models;
using DeerTier.Web.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeerTier.Web.Services
{
    public class CategoryService
    {
        public CategoryService(LeaderboardRepository leaderboardRepository)
        {
            Categories = leaderboardRepository.GetCategories();
            
            CategoryModels = Categories
                .Select(MapCategory)
                .ToArray();

            SectionModels = Categories
                .Where(c => c.Section != null && c.Visible)
                .Select(c => c.Section)
                .Distinct()
                .OrderBy(s => s.Id)
                .Select(MapSection)
                .ToArray();

            CategoryModelsById = new Dictionary<int, CategoryModel>();

            foreach (var categoryModel in CategoryModels)
            {
                CategoryModelsById[categoryModel.Id] = categoryModel;
            }

            CategoriesByUrlName = new Dictionary<string, Category>(StringComparer.OrdinalIgnoreCase);

            foreach (var category in Categories)
            {
                if (!string.IsNullOrEmpty(category.UrlName))
                {
                    CategoriesByUrlName[category.UrlName] = category;
                }
            }
        }

        public SectionModel[] SectionModels { get; private set; }

        private Category[] Categories { get; set; }

        private CategoryModel[] CategoryModels { get; set; }

        private IDictionary<int, CategoryModel> CategoryModelsById { get; set; }

        private IDictionary<string, Category> CategoriesByUrlName { get; set; }

        public Category GetCategory(int id)
        {
            return Categories.FirstOrDefault(c => c.Id == id);
        }

        public Category GetCategoryByUrlName(string urlName)
        {
            Category category;
            CategoriesByUrlName.TryGetValue(urlName, out category);
            return category;
        }
        
        public CategoryModel GetCategoryModel(int id)
        {
            CategoryModel categoryModel;
            CategoryModelsById.TryGetValue(id, out categoryModel);
            return categoryModel;
        }

        private SectionModel MapSection(Section section)
        {
            var sectionModel = new SectionModel
            {
                Name = section.Name,
                Categories = section.Categories
                    .Where(c => c.Visible)
                    .Select(c => CategoryModels.FirstOrDefault(i => i.Id == c.Id))
                    .ToArray()
            };

            return sectionModel;
        }

        private CategoryModel MapCategory(Category category)
        {
            var categoryModel = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                ShortName = category.ShortName,
                UrlName = category.UrlName,
                GameTime = category.GameTime,
                EscapeGameTime = category.EscapeGameTime,
                RealTime = category.RealTime,
                WikiUrl = category.WikiUrl
            };

            if (category.Parent != null)
            {
                categoryModel.FullName = $"{category.Parent.Name} {category.Name}";
                categoryModel.SectionName = category.Parent.Section.Name;

                if (string.IsNullOrEmpty(categoryModel.WikiUrl))
                {
                    categoryModel.WikiUrl = category.Parent.WikiUrl;
                }
            }
            else
            {
                categoryModel.FullName = $"{category.Name}";
                categoryModel.SectionName = category.Section.Name;
            }

            if (string.IsNullOrEmpty(categoryModel.UrlName) && category.DefaultSubcategory != null)
            {
                categoryModel.UrlName = category.DefaultSubcategory.UrlName;
            }

            if (!string.IsNullOrEmpty(categoryModel.UrlName))
            {
                categoryModel.LinkUrl = $"/Leaderboard/{categoryModel.UrlName}";
            }

            return categoryModel;
        }
    }
}