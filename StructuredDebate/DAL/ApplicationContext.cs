using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructuredDebate.Models;
using System.Data.Entity;

namespace StructuredDebate.DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("ApplicationContext")
        {
            Database.SetInitializer<ApplicationContext>(new DropCreateDatabaseAlways<ApplicationContext>());
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Argument> Arguments { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CrossExamination> CrossExaminations { get; set; }
        public DbSet<TagRelation> TagRelations { get; set; }
    }
}