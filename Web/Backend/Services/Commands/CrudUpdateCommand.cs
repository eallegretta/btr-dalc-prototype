using System.ComponentModel.DataAnnotations;

namespace Web.Backend.Services.Commands
{
    public abstract class CrudUpdateCommand<T> : RepositoryCommand<T>  where T: class, new()
    {
        [Required]
        public object Id { get; set; }

        private T _instance;

        protected T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Repository.Get(Id);
                }

                return _instance;
            }
        }

        public override bool Validate()
        {
            bool isValid = base.Validate();

            if (isValid)
            {
                if (Instance == null)
                {
                    AddValidationResult("The " + typeof(T).Name + " with id: " + Id + " does not exist", "Id");
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}