using catalog.infrastructure.Entities;

namespace catalog.infrastructure
{
    public static class CatalogContextExtensions
    {

        public static void InitializeInMemoryDatabase(this CatalogContext context)
        {
            context.Database.EnsureCreated();

            var itemsForFirst = new List<Item>
            {
                new Item { Name = "Item-1-1", Description = "Item 1 is created Category-1", Price = 12.34},
                new Item { Name = "Item-1-2", Description = "Item 2 is created Category-1", Price = 2.34},
                new Item { Name = "Item-1-3", Description = "Item 3 is created Category-1", Price = 1.34},
                new Item { Name = "Item-1-4", Description = "Item 4 is created Category-1", Price = 5.34},
                new Item { Name = "Item-1-5", Description = "Item 5 is created Category-1", Price = 20.34},
            };

            var firstCategory = new Category
            {
                Name = "Category-1",
                Items = itemsForFirst
            };

            var itemsForSecond = new List<Item>
            {
                new Item { Name = "Item-2-1", Description = "Item 1 is created Category-2", Price = 12.34},
                new Item { Name = "Item-2-2", Description = "Item 2 is created Category-2", Price = 2.34},
                new Item { Name = "Item-2-3", Description = "Item 3 is created Category-2", Price = 1.34},
                new Item { Name = "Item-2-4", Description = "Item 4 is created Category-2", Price = 5.34},
                new Item { Name = "Item-2-5", Description = "Item 5 is created Category-2", Price = 20.34},
            };

            var secondCategory = new Category
            {
                Name = "Category-2",
                Items = itemsForSecond
            };

            var itemsForThird = new List<Item>
            {
                new Item { Name = "Item-3-1", Description = "Item 1 is created Category-3", Price = 12.34},
                new Item { Name = "Item-3-2", Description = "Item 2 is created Category-3", Price = 2.34},
                new Item { Name = "Item-3-3", Description = "Item 3 is created Category-3", Price = 1.34},
                new Item { Name = "Item-3-4", Description = "Item 4 is created Category-3", Price = 5.34},
                new Item { Name = "Item-3-5", Description = "Item 5 is created Category-3", Price = 20.34},
            };

            var thirdCategory = new Category
            {
                Name = "Category-3",
                Items = itemsForThird
            };

            context.Categories.AddRange(firstCategory, secondCategory, thirdCategory);
            context.SaveChanges();
        }
    }
}
