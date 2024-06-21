using System.Collections.Immutable;
using Ucommerce.Web.Infrastructure.Core.Models;
using Ucommerce.Web.Infrastructure.Core;
using Ucommerce.Web.Infrastructure;
using Ucommerce.Web.Common.Extensions;

namespace WildBearAdventures.API.ImageService
{
    /// <inheritdoc />
    public class CoffeeImageService : IImageService
    {
        /// <inheritdoc />
        public virtual Task<IImmutableList<Content>> Get(string? parentId = null, int startAt = 0, int limit = 30, CancellationToken? token = default)
        {
            return Task.FromResult<IImmutableList<Content>>(FetchData(null, parentId, startAt, limit)
                .ToImmutableList());
        }

        /// <inheritdoc />
        public virtual Task<Content> GetById(string? id, CancellationToken token)
        {
            var result = FetchData(id: id)
                .FirstOrDefault();

            return result == null
                ? ImageNotFound()
                    .InTask()
                : result.InTask();
        }

        /// <inheritdoc />
        public virtual Task<IImmutableList<Content>> GetByIds(IImmutableList<string> ids, CancellationToken? token = default)
        {
            return Task.FromResult<IImmutableList<Content>>(FetchData()
                .Where(x => ids.Contains(x.Id))
                .ToImmutableList());
        }

        /// <summary>
        /// Gives a default image when the request image could not be found
        /// </summary>
        protected virtual Content ImageNotFound()
        {
            return new Content
            {
                Id = "image-not-found",
                Name = "image-not-found.png",
                Url = "ImageNotFoundImageURL",
                NodeType = Constants.ImagePicker.Image
            };
        }

        private List<Content> FetchData(string? id = null, string? parentId = null, int startAt = 0, int limit = 30)
        {
            //Fetch data from external source here (Digital asset management or other), and parse it to the Content objects if needed
            //For test purposes, the list is hardcoded with test data in this case
            var data = TestData.AsEnumerable();
            if (id != null)
            {
                data = data.Where(x => string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase));
            }

            if (parentId != null)
            {
                data = data.Where(x => string.Equals(x.ParentId, parentId, StringComparison.InvariantCultureIgnoreCase));
            }

            return data.Skip(startAt)
                .Take(limit)
                .ToList();
        }

        /// <summary>
        /// Test data used in place of actually fetched data
        /// </summary>
        public List<Content> TestData = new List<Content>
        {
            new Content
            {
                Icon =
                    "icon-folder",
                Id = "14E6FB8C-F337-4BEA-BA8A-D85A703F9177",
                Name = "Root",
                NodeType = Constants.ImagePicker.Folder
            },
            new Content
            {
                Icon =
                    "icon-folder",
                Id = "D7E67218-6957-4578-9F04-73E0E709CFE1",
                Name = "Coffee drinks",
                NodeType = Constants.ImagePicker.Folder,
                ParentId = "14E6FB8C-F337-4BEA-BA8A-D85A703F9177",
                ChildrenCount = 3
            },
            new Content
            {
                Icon =
                    "icon-folder",
                Id = "BA13CA13-833C-475B-93A7-B5917DAEC74A",
                Name = "Cats",
                NodeType = Constants.ImagePicker.Folder,
                ParentId = "14E6FB8C-F337-4BEA-BA8A-D85A703F9177",
                ChildrenCount = 5
            },
            new Content
            {
                Icon =
                    "icon-picture",
                Id = "AB934A9E-1258-48DC-AB17-820C5B433733",
                Name = "White cup with coffee beans",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.unsplash.com/photo-1610632380989-680fe40816c6?q=80&w=2187&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "D7E67218-6957-4578-9F04-73E0E709CFE1"
            },
            new Content
            {
                Icon =
                    "icon-picture",
                Id = "B63E2D30-4BB4-400D-9095-EB220DAEDFB1",
                Name = "Brown coffee beans",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.unsplash.com/photo-1509785307050-d4066910ec1e?q=80&w=2965&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "D7E67218-6957-4578-9F04-73E0E709CFE1"
            },
            new Content
            {
                Icon =
                    "icon-picture",
                Id = "CD5939CA-8105-4E9F-AE49-50450A3CA884",
                Name = "Ice latte",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?q=80&w=3869&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "D7E67218-6957-4578-9F04-73E0E709CFE1"
            },
            new Content
            {
                Icon =
                    "icon-picture",
                Id = "0D7D6DC5-9721-4C7B-81B6-2FA913727837",
                Name = "Cat1",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.unsplash.com/photo-1561336313-0bd5e0b27ec8?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "BA13CA13-833C-475B-93A7-B5917DAEC74A"
            },
            new Content
            {
                Icon = "icon-picture",
                Id = "35047BCB-64D8-4DEF-8575-8EB341EE4225",
                Name = "Cat2",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.pexels.com/photos/4612722/pexels-photo-4612722.jpeg?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "BA13CA13-833C-475B-93A7-B5917DAEC74A"
            },
            new Content
            {
                Icon = "icon-picture",
                Id = "5767E6D9-64AF-4377-B09E-90AA0AFB1FDA",
                Name = "Cat3",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.pexels.com/photos/16804467/pexels-photo-16804467/free-photo-of-a-white-and-orange-car-on-a-wall.jpeg?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "BA13CA13-833C-475B-93A7-B5917DAEC74A"
            },
            new Content
            {
                Icon = "icon-picture",
                Id = "0388B480-DDA8-481C-94D6-03DF82F58143",
                Name = "Cat4",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.pexels.com/photos/2361952/pexels-photo-2361952.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                ChildrenCount = 0,
                ParentId = "BA13CA13-833C-475B-93A7-B5917DAEC74A"
            },
            new Content
            {
                Icon =
                    "icon-picture",
                Id = "4CFCA2F0-87E7-4D99-9D98-C00AE93D322A",
                Name = "Cat5",
                NodeType = Constants.ImagePicker.Image,
                Url =
                    "https://images.pexels.com/photos/17006168/pexels-photo-17006168/free-photo-of-close-up-of-maine-coon.jpeg?auto=compress&cs=tinysrgb&w=1600",
                ChildrenCount = 0,
                ParentId = "BA13CA13-833C-475B-93A7-B5917DAEC74A"
            }
        };


    }



}
