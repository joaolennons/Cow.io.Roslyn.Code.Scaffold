namespace Templates
{
    public static class ActionExtensions
    {
        public static string ToPastForm(this Action action)
        {
            return $"{action.ToString()}d";
        }
    }
}
