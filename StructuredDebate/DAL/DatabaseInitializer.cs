using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using StructuredDebate.Models;
using FizzWare.NBuilder;
using System.Data.Entity.Migrations;
using FizzWare.NBuilder.Dates;

namespace StructuredDebate.DAL
{
    public class DatabaseInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        private bool BooleanGenerator()
        {
            var r = new Random();
            var num = r.Next(0,2);
            if (num == 0) {
                return false;
            }
            return true;
        }

        protected override void Seed(ApplicationContext context)
        {

            var generator = new RandomGenerator();

            var posts = Builder<Post>.CreateListOfSize(100).All()
                .With(c => c.Claim = Faker.Lorem.Sentence())
                .With(c => c.OpeningStatement = Faker.Lorem.Paragraph())
                .With(c => c.Date = generator.Next(January.The(1), December.The(31)))
                .With(c => c.Score = Faker.RandomNumber.Next(-100, 10000))
                .Build();

            context.Posts.AddOrUpdate(c => c.PostID, posts.ToArray());
            context.SaveChanges();

            var arguments = Builder<Argument>.CreateListOfSize(1000).All()
                .With(c => c.Body = Faker.Lorem.Paragraph())
                .With(c => c.Affirmative = BooleanGenerator())
                .With(c => c.Score = Faker.RandomNumber.Next(-100, 10000))
                .With(c => c.PostID = Faker.RandomNumber.Next(1, 100))
                .Build();

            context.Arguments.AddOrUpdate(c => c.ArgumentID, arguments.ToArray());
            context.SaveChanges();

            var crossExaminations = Builder<CrossExamination>.CreateListOfSize(500).All()
                .With(c => c.Body = Faker.Lorem.Paragraph())
                .With(c => c.Score = Faker.RandomNumber.Next(-100, 10000))
                .With(c => c.ArgumentID = Faker.RandomNumber.Next(1, 1000))
                .Build();

            context.CrossExaminations.AddOrUpdate(c => c.CrossExaminationID, crossExaminations.ToArray());
            context.SaveChanges();

            var sources = Builder<Source>.CreateListOfSize(1000).All()
                .With(c => c.ArgumentID = null)
                .With(c => c.CrossExaminationID = null)
                .With(c => c.URL = Faker.Internet.DomainName())
                .With(c => c.PublishedDate = generator.Next(January.The(1), December.The(31)))
                .With(c => c.Author = Faker.Name.FullName())
                .With(c => c.Description = Faker.Lorem.Sentence())
              .TheFirst(500)
                .With(c => c.ArgumentID = Faker.RandomNumber.Next(1,1000))
              .TheLast(500)
                .With(c => c.CrossExaminationID = Faker.RandomNumber.Next(1, 500))
              .Build();

            context.Sources.AddOrUpdate(c => c.SourceID, sources.ToArray());
            context.SaveChanges();

            var tags = Builder<Tag>.CreateListOfSize(100).Build();

            context.Tags.AddOrUpdate(c => c.TagID, tags.ToArray());
            context.SaveChanges();

            var tagRelations = Builder<TagRelation>.CreateListOfSize(500).All()
                .With(c => c.TagID = Faker.RandomNumber.Next(1, 100))
                .With(c => c.PostID = Faker.RandomNumber.Next(1, 100))
                .Build();

            context.TagRelations.AddOrUpdate(c => c.TagRelationID, tagRelations.ToArray());
            context.SaveChanges();
        }
    }
}