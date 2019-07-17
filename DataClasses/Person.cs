namespace DavidTielke.PersonManagementApp.CrossCutting.DataClasses
{
    public class Person : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
