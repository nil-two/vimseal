using System;

namespace Contents
{
    public class CheckListItem
    {
        public string Name { get; }
        public bool Passed { get; private set; }

        private readonly Func<CheckListVault, bool> _checkPassed;

        public CheckListItem(string name, Func<CheckListVault, bool> checkPassed)
        {
            Name = name;
            Passed = false;
            _checkPassed = checkPassed;
        }

        public void CheckIfPassedAndUpdate(CheckListVault vault)
        {
            if (!Passed && _checkPassed(vault))
            {
                Passed = true;
            }
        }
    }
}
