namespace TrackIt.UI.Model
{
    public class Entry
    {
        public bool Type; //If its true its Expense, if its false its Income.
        public string Description;
        public int Amount;
        public DateTime Date;
        public string Category;
        public string? Period; //If (Period = null) it's a unique Expense/Income entry.
    }
}
