namespace Contents
{
    public class Course
    {
        public Section[] Sections { get; }

        private Course(Section[] sections)
        {
            Sections = sections;
        }

        public static Course BuildDefaultCourse()
        {
            return new Course(
                new Section[]
                {
                    new (
                        Section.SectionIdOfEntry,
                        "ようこそ",
                        "",
                        ""
                        + "ようこそvimsealへ。\n"
                        + "Vimの基本操作を習得していきましょう。\n"
                        + "\n"
                        + "<Enter>を入力して次の画面に進んでください。",
                        new CheckListItem[] { },
                        new CheckListVault()
                    ),
                    new(
                        Section.SectionIdOfBasicMoving,
                        "移動",
                        ""
                        + "This is a sample text.\n"
                        + "You can move the cursor on the editor with keystrokes.\n"
                        + "(h: left, j: bottom, k: top, l: right)",
                        ""
                        + "Vimではh、j、k、lを入力することで、それぞれ左、下、上、右にカーソルを動かすことができます。\n"
                        + "早速やってみましょう。画面右上のチェックリストはそれぞれ条件を満たすと完了になり、全て満たすと<Enter>で次に進めます。\n"
                        + "\n"
                        + "(覚え方: jは文字が下に伸びているため下、kは文字が上に伸びているため上、h、lはjとkの両隣で、hは左側なので左、lは右側なので右)",
                        new CheckListItem[]
                        {
                            new("hで左に移動", (vault => vault.MovedWithH)),
                            new("jで下に移動", (vault => vault.MovedWithJ)),
                            new("kで上に移動", (vault => vault.MovedWithK)),
                            new("lで右に移動", (vault => vault.MovedWithL)),
                        },
                        new CheckListVault()
                    ),
                    new (
                        Section.SectionIdOfWorkInProgress,
                        "工事中",
                        "Work in progress...",
                        ""
                        + "今遊べるのはここまでです。予定だともうちょっと遊べる感じだったんですが、ごめんね…\n"
                        + "まだまだ更新予定なので、更新したらTwitterにてお伝えします(@nil_two)。\n"
                        + "\n"
                        + "<Enter>を入力するとタイトルに戻ります。",
                        new CheckListItem[] { },
                        new CheckListVault()
                    ),
                }
            );
        }
    }
}
