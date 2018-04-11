namespace Common.Exceptions
{
    public class FoodsError
    {
        public string PropertyName { get; set; }
        public object AttemptedValue { get; set; }
        public string ErrorMessage { get; set; }
    }
}
