using MonoMod.RuntimeDetour;
using Quintessential;
using Quintessential.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using static MonoMod.InlineRT.MonoModRule;
using PartType = class_139;
using Permissions = enum_149;
using Song = class_186;
using Texture = class_256;

namespace MetalQuintessenceJournal;

public class MetalQuintessenceJournal: QuintessentialMod
{
    private static IDetour hook_JournalScreen_method_1040;
    public static MethodInfo PrivateMethod<T>(string method) => typeof(T).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    public override void Load()
    {
        hook_JournalScreen_method_1040 = new Hook(MetalQuintessenceJournal.PrivateMethod<JournalScreen>("method_1040"), OnJournalScreen_Method_1040);
    }
    private delegate void orig_JournalScreen_method_1040(JournalScreen screen_self, Puzzle puzzle, Vector2 basePosition, bool isLargePuzzle);
    public override void PostLoad()
    {
        
    }
    public override void Unload()
    {
        hook_JournalScreen_method_1040.Dispose();
        // Blank
    }
    // Puzzle param_4694, Vector2 param_3405, bool param_4695
    private static void OnJournalScreen_Method_1040(orig_JournalScreen_method_1040 orig, JournalScreen screen_self, Puzzle puzzle, Vector2 basePosition, bool isLargePuzzle)
    {
        var puzzleID = puzzle.field_2766;
        if (puzzleID == "mqj-multi-use-process") // do a specific rendering if the id of the puzzle are this
        {
            bool puzzleSolved = GameLogic.field_2434.field_2451.method_573(puzzle);
            var crimson_15 = class_238.field_1990.field_2144;
            bool authorExists = puzzle.field_2768.method_1085();
            string authorName() => puzzle.field_2768.method_1087();
            string displayString = authorExists ? string.Format("{0} ({1})", puzzle.field_2767, authorName()) : (string)puzzle.field_2767;

            Texture moleculeBackdrop = isLargePuzzle ? class_238.field_1989.field_88.field_894 : class_238.field_1989.field_88.field_895;
            Texture divider = isLargePuzzle ? class_238.field_1989.field_88.field_892 : class_238.field_1989.field_88.field_893;
            Texture solvedCheckbox = puzzleSolved ? class_238.field_1989.field_96.field_879 : class_238.field_1989.field_96.field_882;
            class_135.method_290(displayString, basePosition + new Vector2(9f, -19f), crimson_15, class_181.field_1718, (enum_0)0, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, new Color(), null, int.MaxValue, false, true);
            Vector2 vector2_1 = basePosition + new Vector2(moleculeBackdrop.field_2056.X - 27, -23f);
            class_135.method_272(solvedCheckbox, vector2_1);
            class_135.method_272(divider, basePosition + new Vector2(isLargePuzzle ? 7f : 7f, -34f));
            class_135.method_272(moleculeBackdrop, basePosition);

            class_256 before = (isLargePuzzle ? class_238.field_1989.field_88.field_894 : class_238.field_1989.field_88.field_895);
            Bounds2 bounds = Bounds2.WithSize(basePosition, before.field_2056.ToVector2());
            bool mouseHover = bounds.Contains(class_115.method_202());
            
            for (int i = 0; i < 3; i++)
            {
                class_135.method_272(Editor.method_928(puzzle.field_2771[i].field_2813, param_4592: false, mouseHover, new Vector2(500f, 500f), isLargePuzzle, struct_18.field_1431).method_1351().field_937, bounds.Min + new Vector2(46f, 197f) + new Vector2(215 * (i % 2), -140 * (i / 2))); //proof of completeness

                // class_135.method_272(Editor.method_928(puzzle.field_2771[j].field_2813, param_4592: false, mouseHover, new Vector2(500f, 500f), isLargePuzzle, 0.8f).method_1351().field_937, bounds.Min + new Vector2(72f, 187f) + new Vector2(195 * (j % 2), -160 * (j / 2))); van berlo pivot
            }
            if (mouseHover && Input.IsLeftClickPressed())
            {
                Song song = class_238.field_1992.field_968;
                Sound fanfare = class_238.field_1991.field_1832;
                Maybe<class_264> maybeStoryPanel =  true ? struct_18.field_1431 : new class_264(puzzleID); ; //this is probably going to crash //fixed it yay

                GameLogic.field_2434.method_946(new PuzzleInfoScreen(puzzle, song, fanfare, maybeStoryPanel));
                class_238.field_1991.field_1821.method_28(1f);
            }
        }
        else
        {
            orig(screen_self, puzzle, basePosition, isLargePuzzle);
            return;
        }
    }
}