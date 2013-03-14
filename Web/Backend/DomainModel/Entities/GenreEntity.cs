using System.Collections.Generic;

namespace Web.Backend.DomainModel.Entities
{
    public class GenreEntity
    {
        public GenreEntity()
        {
            CategoryTopics = new List<CategoryTopicEntity>();
            ChildGenres = new List<GenreEntity>();
        }

        public virtual int GenreId { get; set; }
        public virtual string GenreDescription { get; set; }

        //private int ParentGenreId { get; set; }
        public virtual GenreEntity ParentGenre { get; set; }
        public virtual int ShowCount { get; set; }
        public virtual int HostCount { get; set; }
        public virtual string GenreUrl { get; set; }

        public virtual IList<CategoryTopicEntity> CategoryTopics { get; protected set; }
        public virtual IList<GenreEntity> ChildGenres { get; protected set; }
    }
}
