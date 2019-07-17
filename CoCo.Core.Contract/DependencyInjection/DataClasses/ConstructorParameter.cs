namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses
{
    public class ConstructorParameter
    {
        public string Name { get; }
        public object Value { get; }

        public ConstructorParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}