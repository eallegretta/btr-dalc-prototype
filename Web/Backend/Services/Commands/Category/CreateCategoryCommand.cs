using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Services.Commands.Category
{
    public class CreateCategoryCommand: RepositoryCommand<GenreEntity>
    {
        [Required(ErrorMessage = "The url is required")]
        public string Url { get; set; }

        [Required(ErrorMessage =  "The description is required")]
        public string Description { get; set; }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }

            var genreEntity = new GenreEntity {GenreUrl = Url, GenreDescription = Description};

            Repository.SaveOrUpdate(genreEntity);

            Result = genreEntity;
        }
    }
}