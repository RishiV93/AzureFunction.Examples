namespace AzureFunction.Examples.Models
{
    public class EntityBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EntityType { get; set; }
        public int YearCreated { get; set; }
    }
}