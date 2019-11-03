using Elements;
using System.Collections.Generic;
using System.Linq;

namespace FormulaDetector
{
    /// <summary>
    /// 論理式バリデータ
    /// </summary>
    public static class FormulaValidator
    {
        /// <summary>
        /// 項型文字列が論理式であるかどうかチェックします。
        /// </summary>
        /// <remarks>
        /// 関数型文字と変数のみで構成された文字列のみチェックします。
        /// </remarks>
        /// <param name="charSequence">文字列(変数、関数、または□のみで構成されたもの)</param>
        /// <returns>
        /// true:入力された文字列は、論理式です。
        /// false:入力された文字列は、論理式ではありません
        /// </returns>
        public static bool Validate(CharSequence charSequence)
        {
            var body = charSequence.Body;
            var argCnts = body.Select(e => e.ArgCount).ToArray();

            Stack<List<Charactor>> stack = new Stack<List<Charactor>>();

            foreach (var chr in body)
            {
                if (chr.Type != Charactor.Types.Value && chr.Type != Charactor.Types.Square)
                {
                    // 変数でも□でもない型の文字のとき、
                    // 現在の部分文字列に同じ型のダミー文字を挿入し、新たな部分文字列を追加する
                    Charactor dummy;
                    switch (chr.Type)
                    {
                        case Charactor.Types.Function:
                            dummy = new Charactor("dummy", Charactor.Types.Function);
                            break;
                        case Charactor.Types.Predicate:
                            dummy = new Charactor("dummy", Charactor.Types.Predicate);
                            break;
                        case Charactor.Types.Conjunction:
                            dummy = new Charactor("dummy", Charactor.Types.Conjunction);
                            break;
                        case Charactor.Types.Quantifier:
                            dummy = new Charactor("dummy", Charactor.Types.Quantifier);
                            break;
                        default:
                            return false;
                    }

                    if (!AddNextCharactor(ref stack, dummy))
                        return false;

                    // 新たにsubStrをPush
                    var newSubStr = new List<Charactor>() { chr };
                    stack.Push(newSubStr);
                }
                else if (chr.Type == Charactor.Types.Value || chr.Type == Charactor.Types.Square)
                {
                    // 変数型文字のとき
                    if (!AddNextCharactor(ref stack, chr))
                        return false;
                }
            }

            // 引数の個数チェック
            foreach (var subStr in stack)
            {
                if (subStr[0].ArgCount + 1 != subStr.Count)
                    return false;
            }

            /* ここまできたら引数の個数チェック完了 */

            // 文字列型チェック
            foreach (var subStr in stack)
            {
                switch (subStr.First().Type)
                {
                    case Charactor.Types.Function:
                    case Charactor.Types.Predicate:

                        if (!subStr.Skip(1).All(e => e.TermPropType == Charactor.TermPropTypes.Term))
                            return false;
                        break;

                    case Charactor.Types.Conjunction:

                        if (!subStr.Skip(1).All(e => e.TermPropType == Charactor.TermPropTypes.Proposition))
                            return false;
                        break;

                    case Charactor.Types.Quantifier:
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 部分文字列stackの適切な位置に文字を追加します。
        /// </summary>
        /// <param name="stack">部分文字列stack</param>
        /// <param name="chr">文字</param>
        /// <returns>処理が成功したならば、true。それ以外のとき、false</returns>
        private static bool AddNextCharactor(ref Stack<List<Charactor>> stack, Charactor chr)
        {
            if (stack.Count == 0)
            {
                var sqare = new Charactor("tmp", Charactor.Types.Square);
                var newSubstr = new List<Charactor>();
                
                newSubstr.Add(sqare);
                stack.Push(newSubstr);
            }

            foreach (var subStr in stack.Select((e, i) => new { Element = e, Index = i }))
            {
                if (subStr.Element[0].ArgCount + 1 <= subStr.Element.Count)
                {
                    // 部分文字列の先頭の引数の個数が、後に続く文字の数と一致しているとき、
                    // この部分文字列をスキップし、次の部分文字列を調べる
                    // ただし、スタックの一番奥に来てしまった場合、エラーとする。
                    if (subStr.Index == stack.Count - 1 && stack.Count != 1)
                        return false;
                    else
                        continue;
                }
                else
                {
                    // 部分文字列の先頭の引数の個数が、後に続く文字列の数より少ない場合、
                    // この変数型文字を部分文字列の末尾に追加する
                    var sqare = new Charactor("tmp", Charactor.Types.Square);
                    if (stack.Count != 0)
                    {
                        subStr.Element.Add(chr);
                    }

                    break;
                }
            }

            return true;
        }
    }

}
