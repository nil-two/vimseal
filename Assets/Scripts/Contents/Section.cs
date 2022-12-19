namespace Contents
{
    public class Section
    {
        public int SectionId { get; }
        public string Title { get; }
        public string Text { get; }
        public string Guide { get; }
        public CheckListItem[] CheckList { get; }
        public CheckListVault CheckListVault { get; }

        public const int SectionIdOfEntry = 1;
        public const int SectionIdOfBasicMoving = 2;
        public const int SectionIdOfWorkInProgress = 3;

        public Section(int sectionId, string title, string text, string guide, CheckListItem[] checkList, CheckListVault checkListVault)
        {
            SectionId = sectionId;
            Title = title;
            Text = text;
            Guide = guide;
            CheckList = checkList;
            CheckListVault = checkListVault;
        }

        public void UpdateCheckListFromCheckListVault()
        {
            foreach (var checkListItem in CheckList)
            {
                checkListItem.CheckIfPassedAndUpdate(CheckListVault);
            }
        }
    }
}
