using POV.Blog.BO;
using POV.Modelo.Blog.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.ContextBlog
{
    public class ContextoBlog: DbContext
    {
		private object firma = null;

		public ContextoBlog(object firma) : base("BlogEngine")
		{
			if (firma == null)
				throw new ArgumentNullException("firma");

			this.firma = firma;

			Database.SetInitializer<ContextoBlog>(null);
			Configuration.LazyLoadingEnabled = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Configurations.Add(new PostBlogEngineMapping());
        }

        public DbSet<PostBlogEngine> PostsBlogEngine { get; set; }

        public int Commit(object firma)
        {
            try
            {
                return (this.firma == firma) ? base.SaveChanges() : 0;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void Disposing(object firma)
        {
            if (this.firma == firma)
                Dispose();
        }


    }
}
